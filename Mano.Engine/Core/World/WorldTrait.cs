using System.Collections.Generic;

namespace mano.Engine
{
    public class WorldTrait : Trait
    {
        // public の前に override を追加してください
        public override void ExecuteInit(Object targetObject, WorldObject worldObject)
        {
            worldObject.Dynamic.Clear();
            worldObject.Dynamic = worldObject.Static.Clone();

            foreach (var obj in worldObject.Dynamic.GetAllObjects())
            {
                obj.Init(worldObject);
            }
        }

        public override void ExecuteUpdate(Object targetObject, WorldObject worldObject)
        {
            foreach (var obj in worldObject.Dynamic.GetAllObjects())
            {
                obj.Update(worldObject);
            }
        }

        public override void ExecuteDispose(Object targetObject, WorldObject worldObject)
        {
            foreach (var obj in worldObject.Dynamic.GetAllObjects())
            {
                obj.Dispose(worldObject);
            }

            worldObject.Dynamic.Clear();
        }
    }
}