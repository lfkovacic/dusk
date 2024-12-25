using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace dusk.mejjiq.util;

public static class JsonUtils
{
    // Converts a collection of primitive types (e.g., int, float, etc.) into a JsonArray
    public static JsonArray ToJsonArray<T>(IEnumerable<T> values)
    {
        var jsonArray = new JsonArray();
        foreach (var value in values)
        {
            jsonArray.Add(JsonValue.Create(value));
        }
        return jsonArray;
    }
    // Converts a JsonArray back into a List<T>
    public static List<T> FromJsonArray<T>(JsonArray jsonArray)
    {
        var list = new List<T>();
        foreach (var item in jsonArray)
        {
            if (item is JsonValue value && value.TryGetValue<T>(out var result))
            {
                list.Add(result);
            }
        }
        return list;
    }
}