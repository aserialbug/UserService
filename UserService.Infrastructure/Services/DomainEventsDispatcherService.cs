using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql.Replication;
using Npgsql.Replication.PgOutput;
using Npgsql.Replication.PgOutput.Messages;
using UserService.Domain.Common;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Services;

public class DomainEventsDispatcherService : BackgroundService
{
    private readonly DomainEventsDispatcherServiceSettings _settings;
    private readonly ILogger<DomainEventsDispatcherService> _logger;
    private readonly SerializationService _serializationService;
    private readonly IServiceProvider _serviceProvider;

    public DomainEventsDispatcherService(IOptions<DomainEventsDispatcherServiceSettings> options,
        ILogger<DomainEventsDispatcherService> logger,
        SerializationService serializationService,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serializationService = serializationService;
        _serviceProvider = serviceProvider;
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

        await foreach (var message in data.WithCancellation(stoppingToken))
        {
            if (message is InsertMessage insertCommand)
            {
                _logger.LogInformation("Got new DomainEvent: {Message}", message.GetType().Name);
                string eventType = default;
                string eventBody = default;
                await foreach (var tuple in insertCommand.NewRow.WithCancellation(stoppingToken))
                {
                    var type = tuple.GetPostgresType();
                    if (type.DisplayName == "jsonb")
                    {
                        eventBody = await tuple.Get<string>(stoppingToken);
                    }
                    else if (type.DisplayName == "text")
                    {
                        eventType = await tuple.Get<string>(stoppingToken);
                    }
                }
                try
                {
                    if (_serializationService.Deserialize(eventType, eventBody) is not DomainEvent instance)
                    {
                        _logger.LogError("Failed to deserialize event {Type}, body {Body}", eventType, eventBody);
                        continue;
                    }
                    await HandleDomainEvent(instance, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to handle domain event: {Message}", e.Message);
                }
            }
            connection.SetReplicationStatus(message.WalEnd);
        }
    }

    private async Task HandleDomainEvent(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(domainEvent, cancellationToken);
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