using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql.Replication;
using Npgsql.Replication.PgOutput;
using Npgsql.Replication.PgOutput.Messages;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Services;

public class DomainEventsDispatcherService : BackgroundService
{
    private readonly DomainEventsDispatcherServiceSettings _settings;
    private readonly ILogger<DomainEventsDispatcherService> _logger;
    private readonly SerializationService _serializationService;
    private readonly IMediator _mediator;
    // private LogicalReplicationConnection? Connection { get; }

    public DomainEventsDispatcherService(IOptions<DomainEventsDispatcherServiceSettings> options, ILogger<DomainEventsDispatcherService> logger, SerializationService serializationService, IMediator mediator)
    {
        _logger = logger;
        _serializationService = serializationService;
        _mediator = mediator;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connectionString = ConnectionString.Parse(_settings.ConnectionString);
        await using var connection = new LogicalReplicationConnection(connectionString.ToString());
        await connection.Open(stoppingToken);

        var slot = new PgOutputReplicationSlot(_settings.ReplicationSlotName);
        var data = connection.StartReplication(
            slot, new PgOutputReplicationOptions(_settings.PublicationName, 1), 
            stoppingToken);
        
        Dictionary<string, (string Type, string Body)>? insertedEvents = default;
        await foreach (var message in data.WithCancellation(stoppingToken))
        {
            if (message is BeginMessage)
            {
                insertedEvents = new Dictionary<string, (string Type, string Body)>();
            }
            else if (message is CommitMessage)
            {
                if (insertedEvents == null)
                {
                    _logger.LogError("Somethings wrong! Got empty COMMIT on DomainEvents");
                    continue;
                }

                await HandleInsertedEvents(insertedEvents, stoppingToken);
                insertedEvents = default;
            }
            else if (message is InsertMessage insertCommand)
            {
                if (insertedEvents == null)
                {
                    _logger.LogError("Somethings wrong! Got INSERT command without BEGIN transaction on DomainEvents");
                    continue;
                }
                _logger.LogInformation("Got new DomainEvent: {Message}", message.GetType().Name);
                string id = default;
                string eventType = default;
                string eventBody = default;
                await foreach (var tuple in insertCommand.NewRow.WithCancellation(stoppingToken))
                {
                    var type = tuple.GetPostgresType();
                    if (type.DisplayName == "uuid")
                    {
                        id = await tuple.Get<string>(stoppingToken);
                    }
                    else if (type.DisplayName == "jsonb")
                    {
                        eventBody = await tuple.Get<string>(stoppingToken);
                    }
                    else if (type.DisplayName == "text")
                    {
                        eventType = await tuple.Get<string>(stoppingToken);
                    }
                }
                insertedEvents.Add(id, (eventType, eventBody));
            }
            else if (message is KeyDeleteMessage)
            {
                _logger.LogWarning("Somethings wrong! Got delete from DomainEvents table");
            }
            connection.SetReplicationStatus(message.WalEnd);
        }
    }

    private async Task HandleInsertedEvents(Dictionary<string, (string Type, string Body)> insertedEvents, CancellationToken cancellationToken)
    {
        await Task.WhenAll(insertedEvents
                .Values
                .Select(v => _serializationService.Deserialize(v.Type, v.Body))
                .Where(i => i != null)
                .Select(e => _mediator.Publish(e, cancellationToken))
            );
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public record DomainEventsDispatcherServiceSettings
    {
        public const string SectionName = "DomainEventsDispatcher";
        public string ConnectionString { get; init; }
        public string PublicationName { get; init; }
        public string ReplicationSlotName { get; init; }
    }
}