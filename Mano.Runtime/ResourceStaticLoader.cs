using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.IO;
using mano.Engine;

namespace mano.Runtime
{
    public static class ObjectFactory
    {
        public static mano.Engine.Object Create(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return new mano.Engine.Object();

            Type? type = GetEngineType(typeName);
            if (type != null && typeof(mano.Engine.Object).IsAssignableFrom(type))
            {
                var instance = Activator.CreateInstance(type) as mano.Engine.Object;
                if (instance != null) return instance;
            }
            
            return new mano.Engine.Object();
        }

        public static Type? GetEngineType(string typeName)
        {
            var assembly = typeof(mano.Engine.Object).Assembly;
            return assembly.GetType($"mano.Engine.{typeName}");
        }
    }

    public class ResourceStaticLoader
    {
        private readonly TraitRegistry _registry;

        public ResourceStaticLoader(TraitRegistry registry)
        {
            _registry = registry;
        }

        public List<mano.Engine.Object> LoadObjects(string relativePath)
        {
            var resultList = new List<mano.Engine.Object>();
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            
            if (!File.Exists(fullPath)) return resultList;

            string jsonContent = File.ReadAllText(fullPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                foreach (JsonElement element in doc.RootElement.EnumerateArray())
                {
                    string typeName = element.GetProperty("ObjectType").GetString() ?? "";
                    mano.Engine.Object newObj = ObjectFactory.Create(typeName);

                    if (element.TryGetProperty("Id", out JsonElement idElement))
                    {
                        newObj.Id = idElement.GetString() ?? "";
                    }

                    PopulateFields(newObj, element, options);

                    if (element.TryGetProperty("Traits", out JsonElement traitsElement))
                    {
                        foreach (JsonElement traitNameElement in traitsElement.EnumerateArray())
                        {
                            string traitId = traitNameElement.GetString() ?? "";
                            if (!string.IsNullOrEmpty(traitId))
                            {
                                var trait = _registry.GetTrait(traitId);
                                if (trait != null) newObj.Traits.Add(traitId);
                            }
                        }
                    }

                    resultList.Add(newObj);
                }
            }
            return resultList;
        }

        public static void PopulateFields(mano.Engine.Object obj, JsonElement json, JsonSerializerOptions options)
        {
            if (!json.TryGetProperty("Fields", out JsonElement fieldsElement)) return;

            foreach (var componentJson in fieldsElement.EnumerateObject())
            {
                string componentName = componentJson.Name; 
                Type? compType = ObjectFactory.GetEngineType(componentName); 
                
                if (compType != null)
                {
                    object? componentInstance = JsonSerializer.Deserialize(componentJson.Value.GetRawText(), compType, options);
                    
                    if (componentInstance != null)
                    {
                        InjectComponent(obj.Fields, compType, componentInstance);
                    }
                }
            }
        }

        private static void InjectComponent(object fieldContainer, Type componentType, object componentInstance)
        {
            Type containerType = fieldContainer.GetType();
            
            foreach (var method in containerType.GetMethods())
            {
                if ((method.Name == "Set" || method.Name == "Add" || method.Name == "Register") && method.IsGenericMethod)
                {
                    var genericMethod = method.MakeGenericMethod(componentType);
                    genericMethod.Invoke(fieldContainer, new[] { componentInstance });
                    return;
                }
            }
        }
    }
}