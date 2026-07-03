namespace mano.Engine
{
    // Traitを継承（他のTraitと同じ扱い）
    public class WorldTrait : Trait
    {
        public override void OnInit(Object targetObject, WorldObject worldObject)
        {
            // 1. まずDynamicの中身をリセット
            worldObject.Dynamic.Clear();

            // 2. StaticからDynamicへ実体化（クローン）
            foreach (var kv in worldObject.Static.Character) worldObject.Dynamic.Character[kv.Key] = kv.Value.Clone();
            foreach (var kv in worldObject.Static.Item) worldObject.Dynamic.Item[kv.Key] = kv.Value.Clone();
            foreach (var kv in worldObject.Static.Room) worldObject.Dynamic.Room[kv.Key] = kv.Value.Clone();
            foreach (var kv in worldObject.Static.Rule) worldObject.Dynamic.Rule[kv.Key] = kv.Value.Clone();

            // 3. 複製した実体たち自身のInitを呼んであげる
            foreach (var obj in worldObject.Dynamic.Character.Values) obj.Init(worldObject);
            foreach (var obj in worldObject.Dynamic.Item.Values) obj.Init(worldObject);
            foreach (var obj in worldObject.Dynamic.Room.Values) obj.Init(worldObject);
            foreach (var obj in worldObject.Dynamic.Rule.Values) obj.Init(worldObject);
        }

        public override void OnUpdate(Object targetObject, WorldObject worldObject, Resource.Dynamic dynamicData)
        {
            // Dynamicにいる実体すべてのUpdateを回す
            foreach (var obj in worldObject.Dynamic.Character.Values) obj.Update(worldObject, dynamicData);
            foreach (var obj in worldObject.Dynamic.Item.Values) obj.Update(worldObject, dynamicData);
            foreach (var obj in worldObject.Dynamic.Room.Values) obj.Update(worldObject, dynamicData);
            foreach (var obj in worldObject.Dynamic.Rule.Values) obj.Update(worldObject, dynamicData);
        }

        public override void OnDispose(Object targetObject, WorldObject worldObject)
        {
            // 終了時に、各オブジェクトのDisposeを呼んであげる
            foreach (var obj in worldObject.Dynamic.Character.Values) obj.Dispose(worldObject);
            foreach (var obj in worldObject.Dynamic.Item.Values) obj.Dispose(worldObject);
            foreach (var obj in worldObject.Dynamic.Room.Values) obj.Dispose(worldObject);
            foreach (var obj in worldObject.Dynamic.Rule.Values) obj.Dispose(worldObject);

            // 最後にDynamic空間を空にする
            worldObject.Dynamic.Clear();
        }
    }
}