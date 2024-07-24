using System.Text.Json;
using System.Text.Json.Serialization;

namespace WPG_HmacAuthentication;

public static class JsonParser
{
    private static readonly JsonSerializerOptions SerializerOptions = new();

    static JsonParser()
    {
        SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        SerializerOptions.PropertyNameCaseInsensitive = true;
    }

    public static string? ToJson(this object? obj)
    {
        try
        {
            return obj == null ? null : JsonSerializer.Serialize(obj, SerializerOptions);
        }
        catch
        {
            return null;
        }
    }
}