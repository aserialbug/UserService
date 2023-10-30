using Newtonsoft.Json;
using NpgsqlTypes;
using UserService.Domain.Common;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Converters;

namespace UserService.Infrastructure.Adapters;

public class DomainEventsDatabaseAdapter
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        Converters = new List<JsonConverter>()
        {
            new UserIdJsonConverter(),
            new DomainEventIdJsonConverter(),
            new PostIdJsonConverter()
        }
    };
    
    private const string AddRangeSql = "insert into domain_events (id, type, content) values (@id, @type, @content)";

    public async Task AddRange(IEnumerable<DomainEvent> domainEvents, DatabaseTransaction transaction)
    {
        if (domainEvents == null) throw new ArgumentNullException(nameof(domainEvents));
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        var values = domainEvents
            .Select(evt =>
            {
                var cmd = transaction.CreateBatchCommand(AddRangeSql);
                cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, evt.Id.ToGuid());
                cmd.Parameters.AddWithValue("type", NpgsqlDbType.Text, evt.GetType().Name);
                cmd.Parameters.AddWithValue("content", NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(evt, SerializerSettings));
                return cmd;
            });

        await using var addRangeCommand = transaction.CreateBatch(values);
        await addRangeCommand.ExecuteNonQueryAsync();
    }
}