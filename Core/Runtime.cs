using System.Text.Json;

namespace mano
{
    public class Runtime : ManoObject
    {
        public static Runtime Instance { get; } = new Runtime();

        public List<ManoObject> InstanceObjects { get; } = new();

        public static List<ManoObject> Objects => Instance.InstanceObjects;

        public static new void Down(string eventId) => ((ManoObject)Instance).Down(eventId);
        public static new void Down(string[] events) => ((ManoObject)Instance).Down(events);

        public static new void Up(string eventId) => ((ManoObject)Instance).Up(eventId);
        public static new void Up(string[] events) => ((ManoObject)Instance).Up(events);

        private readonly Stack<ManoObject> _callStack = new();

        internal void PushCall(ManoObject obj) => _callStack.Push(obj);

        internal void PopCall() => _callStack.Pop();

        internal ManoObject? GetCallerOf(ManoObject obj)
        {
            if (_callStack.Count == 0 || !ReferenceEquals(_callStack.Peek(), obj))
                return null;

            var frames = _callStack.ToArray();
            return frames.Length > 1 ? frames[1] : null;
        }

        private readonly HashSet<(ManoObject, string)> _processingGuard = new();

        internal bool TryEnterEvent(ManoObject obj, string eventId) => _processingGuard.Add((obj, eventId));

        internal void ExitEvent(ManoObject obj, string eventId) => _processingGuard.Remove((obj, eventId));

        public static List<ManoObject> Load(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using var doc = JsonDocument.Parse(json);
            var result = new List<ManoObject>();

            foreach (var element in doc.RootElement.EnumerateArray())
            {
                if (!element.TryGetProperty("ObjectType", out var typeProp))
                {
                    throw new InvalidOperationException("The JSON element is missing the 'ObjectType' property.");
                }

                string typeName = typeProp.GetString()!;
                Type? targetType = FindTypeByName(typeName);
                if (targetType == null)
                {
                    throw new InvalidOperationException($"The type '{typeName}' was not found.");
                }

                var manoObj = JsonSerializer.Deserialize(element.GetRawText(), targetType, options) as ManoObject;
                if (manoObj == null) continue;

                if (element.TryGetProperty("Traits", out var traitsProp) && traitsProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var traitElement in traitsProp.EnumerateArray())
                    {
                        string? traitName = traitElement.GetString();
                        if (!string.IsNullOrEmpty(traitName))
                        {
                            var trait = TraitRegistry.Get(traitName);
                            if (trait != null)
                            {
                                manoObj.AttachTrait(trait);
                            }
                        }
                    }
                }

                result.Add(manoObj);
            }

            return result;
        }

        private static Type? FindTypeByName(string typeName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType($"mano.{typeName}") ?? assembly.GetType(typeName);
                if (type != null) return type;
            }
            return null;
        }
    }
}