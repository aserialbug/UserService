using Newtonsoft.Json;
using UserService.Domain.Posts;

namespace UserService.Infrastructure.Converters;

public class PostIdJsonConverter : JsonConverter<PostId>
{
    public override void WriteJson(JsonWriter writer, PostId? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override PostId? ReadJson(JsonReader reader, Type objectType, PostId? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return PostId.Parse(reader.ReadAsString());
    }
}