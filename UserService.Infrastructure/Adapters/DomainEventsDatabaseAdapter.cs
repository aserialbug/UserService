using Newtonsoft.Json;
using NpgsqlTypes;
using UserService.Domain.Common;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Adapters;

public class DomainEventsDatabaseAdapter
{
    private readonly SerializationService _serializationService;

    private const string AddRangeSql = "insert into domain_events (id, type, content) values (@id, @type, @content)";

    public DomainEventsDatabaseAdapter(SerializationService serializationService)
    {
        _serializationService = serializationService;
    }

    public async Task AddRange(IEnumerable<DomainEvent> domainEvents, DatabaseTransaction transaction)
    {
        if (domainEvents == null) throw new ArgumentNullException(nameof(domainEvents));
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        var values = domainEvents
            .Select(evt =>
            {
                var cmd = transaction.CreateBatchCommand(AddRangeSql);
                cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, evt.Id.ToGuid());
                cmd.Parameters.AddWithValue("type", NpgsqlDbType.Text, evt.GetType().AssemblyQualifiedName);
                cmd.Parameters.AddWithValue("content", NpgsqlDbType.Jsonb, _serializationService.Serialize(evt));
                return cmd;
            });

        await using var addRangeCommand = transaction.CreateBatch(values);
        await addRangeCommand.ExecuteNonQueryAsync();
    }
}