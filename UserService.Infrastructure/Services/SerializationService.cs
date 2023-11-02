using Newtonsoft.Json;
using UserService.Infrastructure.Converters;

namespace UserService.Infrastructure.Services;

public class SerializationService
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        Converters = new List<JsonConverter>
        {
            new UserIdJsonConverter(),
            new DomainEventIdJsonConverter(),
            new PostIdJsonConverter()
        }
    };

    public string Serialize(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        return JsonConvert.SerializeObject(obj, SerializerSettings);
    }

    public object? Deserialize(string typeName, string body)
    {
        var typeInstance = Type.GetType(typeName);
        return typeInstance == null ? null : JsonConvert.DeserializeObject(body, typeInstance, SerializerSettings);
    }
}