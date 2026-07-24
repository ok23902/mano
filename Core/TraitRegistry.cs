using System.Collections.Concurrent;
using System.Reflection;

namespace mano
{
    public static class TraitRegistry
    {
        private static readonly ConcurrentDictionary<Type, ManoTrait> Registry = new();

        static TraitRegistry()
        {
            AutoRegister();
        }

        public static T Get<T>() where T : ManoTrait
        {
            if (Registry.TryGetValue(typeof(T), out var trait))
            {
                return (T)trait;
            }
            throw new InvalidOperationException($"Trait {typeof(T).Name} is not registered.");
        }

        public static ManoTrait? Get(string traitName)
        {
            foreach (var kvp in Registry)
            {
                if (kvp.Key.Name == traitName) return kvp.Value;
            }
            return null;
        }

        public static void AutoRegister()
        {
            var traitType = typeof(ManoTrait);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                if (assembly.FullName != null && assembly.FullName.StartsWith("System")) continue;

                Type?[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }

                foreach (var type in types)
                {
                    if (type != null && traitType.IsAssignableFrom(type) && !type.IsAbstract && type.IsClass)
                    {
                        try
                        {
                            var instance = (ManoTrait)Activator.CreateInstance(type)!;
                            Registry[type] = instance;
                        }
                        catch { }
                    }
                }
            }
        }
    }
}