using System;
using System.Collections.Generic;
using mano.Engine.Core;

namespace mano.Runtime
{
    public class ResourceLoader
    {
        private readonly Runtime _runtime;

        public ResourceLoader(Runtime runtime)
        {
            _runtime = runtime;
        }

        // ジェネリクス型引数(T)を使い、辞書ではなく継承先に直書きされた具象Objectを生成する
        public T LoadAndSpawnObject<T>(string id, string name, List<string>? traitsToAttach = null) where T : Engine.Core.Object, new()
        {
            // 1. 直書きフィールドを持つ具象クラスのインスタンス生成
            var newObj = new T
            {
                Id = id,
                Name = name
            };

            // 2. シンプルなInitの呼び出し
            newObj.Init();

            // 3. Traitのアタッチ
            if (traitsToAttach != null)
            {
                foreach (var traitName in traitsToAttach)
                {
                    _runtime.AttachTraitToObject(traitName, newObj);
                }
            }

            return newObj;
        }
    }
}