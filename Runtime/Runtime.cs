using System;
using System.Collections.Generic;
using mano.Engine.Core;

namespace mano.Runtime
{
    public class Runtime
    {
        public WorldObject World { get; private set; }
        private readonly Dictionary<string, Trait> _traitRegistry = new Dictionary<string, Trait>();

        // WorldObjectは外部（ResourceLoader等）で生成して渡す
        public Runtime(WorldObject world)
        {
            World = world ?? throw new ArgumentNullException(nameof(world));
        }

        // 起動時に全Traitを登録（Registry）
        public void RegisterTrait(Trait trait)
        {
            if (!_traitRegistry.ContainsKey(trait.Id))
            {
                _traitRegistry[trait.Id] = trait;
            }
        }

        // アタッチ処理（リストにIdを追加してOnInitを呼ぶだけ）
        public void AttachTraitToObject(string traitName, mano.Engine.Core.Object targetObject)
        {
            if (!_traitRegistry.ContainsKey(traitName))
            {
                throw new KeyNotFoundException($"Trait '{traitName}' は登録されていません。");
            }

            if (!targetObject.Traits.Contains(traitName))
            {
                targetObject.Traits.Add(traitName);
                _traitRegistry[traitName].OnInit(targetObject, World);
            }
        }

        // Avalonia側のタイマー（Update）から毎フレーム呼ばれる
        public void Step()
        {
            World.Update();
        }

        // Object側（Update内）から自身のTraitsリストを元に呼び出されるヘルパー
        public void ExecuteObjectTrait(mano.Engine.Core.Object obj, string traitName)
        {
            if (_traitRegistry.TryGetValue(traitName, out var trait))
            {
                trait.OnUpdate(obj, World);
            }
        }
    }
}