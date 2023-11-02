using Newtonsoft.Json;
using UserService.Domain.Common;

namespace UserService.Infrastructure.Converters;

public class DomainEventIdJsonConverter : JsonConverter<DomainEventId>
{
    public override void WriteJson(JsonWriter writer, DomainEventId? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override DomainEventId? ReadJson(JsonReader reader, Type objectType, DomainEventId? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return DomainEventId.Parse(reader.Value.ToString());
    }
}