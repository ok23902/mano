using System.Text.Json;

namespace mano;

public static class Mano
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static ManoObject[] Load(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var list = new List<ManoObject>();
        foreach (var el in doc.RootElement.EnumerateArray())
            list.Add(LoadOne(el));
        return list.ToArray();
    }

    public static T[] Load<T>(string json) where T : ManoObject
        => Load(json).OfType<T>().ToArray();

    private static ManoObject LoadOne(JsonElement el)
    {
        var typeName = ReadString(el, "type")
            ?? throw new InvalidOperationException("Missing 'type' property.");

        var target = ResolveType(typeName)
            ?? throw new InvalidOperationException($"Type '{typeName}' was not found.");

        var obj = (ManoObject?)JsonSerializer.Deserialize(el.GetRawText(), target, Options)
            ?? throw new InvalidOperationException($"Failed to deserialize '{typeName}'.");

        if (el.TryGetProperty("traits", out var traitsEl) && traitsEl.ValueKind == JsonValueKind.Array)
        {
            foreach (var t in traitsEl.EnumerateArray())
            {
                var name = t.GetString();
                if (string.IsNullOrEmpty(name)) continue;
                var trait = TraitRegistry.Get(name);
                if (trait != null) obj.Attach(trait);
            }
        }

        return obj;
    }

    private static string? ReadString(JsonElement el, string name)
    {
        foreach (var p in el.EnumerateObject())
            if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
                return p.Value.GetString();
        return null;
    }

    private static Type? ResolveType(string name)
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            var t = asm.GetType($"mano.{name}") ?? asm.GetType(name);
            if (t != null) return t;
        }
        return null;
    }
}