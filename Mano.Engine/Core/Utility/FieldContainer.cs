using System;
using System.Collections.Generic;
using System.Text.Json;

namespace mano.Engine
{
    public class FieldContainer
    {
        private readonly Dictionary<Type, object> _components = new Dictionary<Type, object>();

        public T Get<T>() where T : class, new()
        {
            var type = typeof(T);
            if (!_components.TryGetValue(type, out var component))
            {
                component = new T();
                _components[type] = component;
            }
            return (T)component;
        }

        public void Set<T>(T value) where T : class
        {
            if (value == null) return;
            _components[typeof(T)] = value;
        }

        public bool Has<T>() where T : class
        {
            return _components.ContainsKey(typeof(T));
        }

        public IEnumerable<object> GetAll()
        {
            return _components.Values;
        }

        public FieldContainer Clone()
        {
            var newContainer = new FieldContainer();
            foreach (var kvp in _components)
            {
                newContainer._components[kvp.Key] = DeepCopyComponent(kvp.Value);
            }
            return newContainer;
        }

        private object DeepCopyComponent(object original)
        {
            if (original == null) return null!;
            var type = original.GetType();
            var json = JsonSerializer.Serialize(original, type);
            return JsonSerializer.Deserialize(json, type)!;
        }
    }
}