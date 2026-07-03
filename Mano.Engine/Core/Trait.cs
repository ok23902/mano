using System.Collections.Generic;

namespace mano.Engine
{
    public abstract class Trait
    {
        public string Id => this.GetType().Name;

        public abstract void ExecuteInit(Object obj, WorldObject worldObject);
        public abstract void ExecuteUpdate(Object obj, WorldObject worldObject);
        public abstract void ExecuteDispose(Object obj, WorldObject worldObject);
    }

    public abstract class Trait<T> : Trait where T : Object
    {
        public virtual void OnInit(T obj, WorldObject worldObject) { }
        public virtual void OnUpdate(T obj, WorldObject worldObject) { }
        public virtual void OnDispose(T obj, WorldObject worldObject) { }

        public override void ExecuteInit(Object obj, WorldObject worldObject)
        {
            if (obj is T typed) OnInit(typed, worldObject);
        }

        public override void ExecuteUpdate(Object obj, WorldObject worldObject)
        {
            if (obj is T typed) OnUpdate(typed, worldObject);
        }

        public override void ExecuteDispose(Object obj, WorldObject worldObject)
        {
            if (obj is T typed) OnDispose(typed, worldObject);
        }
    }
}