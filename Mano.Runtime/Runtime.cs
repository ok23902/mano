using System.Collections.Generic;
using mano.Engine;

namespace mano.Runtime
{
    public class Runtime
    {
        private Resource _resource;
        private TraitRegistry _registry;
        public WorldObject WorldObject { get; private set; }

        public void Init()
        {
            _resource = new Resource();
            _registry = new TraitRegistry();
            
            // TODO: ここで必要なTraitを手動でレジストリに登録する
            // _registry.Register(new MoveTrait()); 

            WorldObject = new WorldObject();
            
            // ローダーの生成
            ResourceStaticLoader loader = new ResourceStaticLoader(_registry);

            // 各JSONから生成したObjectを、直接対応するStatic内の辞書に詰める
            LoadAndStore(loader, "Resource/Static/character.json", WorldObject.Static.Character);
            LoadAndStore(loader, "Resource/Static/item.json", WorldObject.Static.Item);
            LoadAndStore(loader, "Resource/Static/room.json", WorldObject.Static.Room);
            LoadAndStore(loader, "Resource/Static/rule.json", WorldObject.Static.Rule);

            // StaticからDynamicへ実体化する
            WorldObject.Init();
        }

        public void Update()
        {
            // Dynamicリソースを渡して更新
            WorldObject.Update(_resource.Dynamic);
        }

        public void Dispose()
        {
            // 必要な終了処理
        }

        // Loaderで読み込んだリストを、指定した辞書に直接格納するヘルパー
        private void LoadAndStore(ResourceStaticLoader loader, string path, Dictionary<string, Engine.Object> targetDict)
        {
            var objects = loader.LoadObjects(path);
            foreach (var obj in objects)
            {
                targetDict[obj.Id] = obj;
            }
        }
    }
}