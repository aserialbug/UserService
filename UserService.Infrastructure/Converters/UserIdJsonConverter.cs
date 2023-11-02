using Newtonsoft.Json;
using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Infrastructure.Converters;

public class UserIdJsonConverter : JsonConverter<UserId>
{
    public override void WriteJson(JsonWriter writer, UserId? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override UserId? ReadJson(JsonReader reader, Type objectType, UserId? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return UserId.Parse(reader.Value.ToString());
    }
}