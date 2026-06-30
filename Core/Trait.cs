using System.Collections.Generic;

namespace mano
{
    public abstract class Trait
    {
        public string Id => this.GetType().Name;

        public abstract void ExecuteInit(Object obj, WorldObject world);
        public abstract void ExecuteUpdate(Object obj, WorldObject world);
        public abstract void ExecuteDispose(Object obj, WorldObject world);
    }

    public abstract class Trait<T> : Trait where T : Object
    {
        public virtual void OnInit(T obj, WorldObject world) { }
        public virtual void OnUpdate(T obj, WorldObject world) { }
        public virtual void OnDispose(T obj, WorldObject world) { }

        public override void ExecuteInit(Object obj, WorldObject world) => OnInit((T)obj, world);
        public override void ExecuteUpdate(Object obj, WorldObject world) => OnUpdate((T)obj, world);
        public override void ExecuteDispose(Object obj, WorldObject world) => OnDispose((T)obj, world);
    }
}