using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.IO;
using mano.Engine;

namespace mano.Runtime
{
    // ファクトリをここに併設（拡張しやすくするため）
    public static class ObjectFactory
    {
        public static mano.Engine.Object Create(string typeName)
        {
            return typeName switch
            {
                "Character" => new CharacterObject(),
                "Item" => new ItemObject(),
                "Room" => new RoomObject(),
                "Rule" => new RuleObject(),
                _ => new mano.Engine.Object()
            };
        }
    }

    public class ResourceStaticLoader
    {
        private readonly TraitRegistry _registry;

        // Loaderには Runtime ではなく、Traitを引っ張るための Registry だけを渡す
        public ResourceStaticLoader(TraitRegistry registry)
        {
            _registry = registry;
        }

        public List<mano.Engine.Object> LoadObjects(string relativePath)
        {
            var resultList = new List<mano.Engine.Object>();
            
            // 実行ファイルの場所を基底パスとして絶対パスを算出
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            
            if (!File.Exists(fullPath)) return resultList; // ファイルが無ければ空を返す

            string jsonContent = File.ReadAllText(fullPath);

            // JSONが配列形式になっている前提: [ { "Id": "Player", "Type": "Character", ... } ]
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                foreach (JsonElement element in doc.RootElement.EnumerateArray())
                {
                    // 1. Typeを取得してFactoryで生成
                    string typeName = element.GetProperty("ObjectType").GetString();
                    mano.Engine.Object newObj = ObjectFactory.Create(typeName);

                    // 2. フィールド/プロパティへの自動代入
                    PopulateObject(newObj, element);

                    // 3. Traitのアタッチ (JSON内に "Traits": ["MoveTrait"] などの配列がある想定)
                    if (element.TryGetProperty("Traits", out JsonElement traitsElement))
                    {
                        foreach (JsonElement traitNameElement in traitsElement.EnumerateArray())
                        {
                            string traitId = traitNameElement.GetString();
                            var trait = _registry.GetTrait(traitNameElement.GetString());
                            if (trait != null) newObj.Traits.Add(traitId);
                        }
                    }

                    // 初期化を呼んでリストに追加
                    resultList.Add(newObj);
                }
            }
            return resultList;
        }

        public static void PopulateObject(mano.Engine.Object obj, JsonElement json)
        {
            var type = obj.GetType();
            
            foreach (var property in json.EnumerateObject())
            {
                var propInfo = type.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propInfo != null && propInfo.CanWrite)
                {
                    object value = property.Value.ValueKind switch
                    {
                        JsonValueKind.String => property.Value.GetString(),
                        JsonValueKind.Number => property.Value.GetDouble(),
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        _ => null
                    };

                    // プロパティの型（intやfloatなど）に安全に変換して代入
                    if (value != null)
                    {
                        propInfo.SetValue(obj, Convert.ChangeType(value, propInfo.PropertyType));
                    }
                }
            }
        }
    }
}