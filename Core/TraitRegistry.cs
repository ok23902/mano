using System.Reflection;

namespace mano;

public static class TraitRegistry
{
    private static readonly Dictionary<Type, ManoTrait> _registry = new();
    private static readonly object _lock = new();
    private static bool _loaded;

    public static void Load()
    {
        lock (_lock)
        {
            if (_loaded) return;
            _loaded = true;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName?.StartsWith("System") == true) continue;

                Type?[] types;
                try { types = asm.GetTypes(); }
                catch (ReflectionTypeLoadException e) { types = e.Types; }

                foreach (var t in types)
                {
                    if (t == null) continue;
                    if (!typeof(ManoTrait).IsAssignableFrom(t)) continue;
                    if (t.IsAbstract || !t.IsClass) continue;

                    try
                    {
                        var instance = (ManoTrait)Activator.CreateInstance(t)!;
                        _registry[t] = instance;
                    }
                    catch {}
                }
            }
        }
    }

    public static T Get<T>() where T : ManoTrait
    {
        if (!_loaded) Load();
        if (_registry.TryGetValue(typeof(T), out var t)) return (T)t;
        throw new InvalidOperationException($"Trait '{typeof(T).Name}' is not registered.");
    }

    public static ManoTrait? Get(string name)
    {
        if (!_loaded) Load();
        foreach (var kv in _registry)
            if (kv.Key.Name == name) return kv.Value;
        return null;
    }

    public static void Register<T>(T instance) where T : ManoTrait
    {
        if (!_loaded) Load();
        _registry[typeof(T)] = instance;
    }
}