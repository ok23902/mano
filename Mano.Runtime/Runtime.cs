using System.Collections.Generic;
using mano.Engine;

namespace mano.Runtime
{
    public class Runtime
    {
        private Resource _resource = new Resource();
        private TraitRegistry _registry = new TraitRegistry();
        public WorldObject WorldObject { get; private set; } = new WorldObject();

        public void Init()
        {
            WorldObject.TraitProvider = _registry;
            
            _registry.Register("WorldTrait", new WorldTrait());
            
            // ローダーの生成
            ResourceStaticLoader loader = new ResourceStaticLoader(_registry);

            // 各JSONから生成したObjectを、直接対応するStatic内の辞書に詰める
            LoadAndStore(loader, "Resource/Static/character.json", WorldObject.Static.Character);
            LoadAndStore(loader, "Resource/Static/item.json", WorldObject.Static.Item);
            LoadAndStore(loader, "Resource/Static/room.json", WorldObject.Static.Room);
            LoadAndStore(loader, "Resource/Static/rule.json", WorldObject.Static.Rule);

            var worldTrait = _registry.GetTrait("WorldTrait");
            worldTrait?.ExecuteInit(new mano.Engine.Object(), WorldObject);
        }

        public void Update()
        {
            var worldTrait = _registry.GetTrait("WorldTrait");
            worldTrait?.ExecuteUpdate(new mano.Engine.Object(), WorldObject);
        }

        public void Dispose()
        {
            // 必要な終了処理
        }

        // Loaderで読み込んだリストを、指定した辞書に直接格納するヘルパー
        private void LoadAndStore(ResourceStaticLoader loader, string path, Dictionary<string, mano.Engine.Object> targetDict)
        {
            var objects = loader.LoadObjects(path);
            foreach (var obj in objects)
            {
                targetDict[obj.Id] = obj;
            }
        }
    }
}