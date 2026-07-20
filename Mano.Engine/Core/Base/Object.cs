using System;

namespace mano.Engine
{
    public class Object
    {
        public string Id { get; set; } = "";
        public List<string> Traits { get; set; } = new List<string>();
        
        public FieldContainer Fields { get; set; } = new FieldContainer();


        public virtual Object Clone()
        {
            var clone = (Object)Activator.CreateInstance(this.GetType())!;
            
            clone.Id = this.Id;
            clone.Fields = this.Fields.Clone();
            
            return clone;
        }

        public void Init(WorldObject world)
        {
            if (world.TraitProvider == null) return;
            foreach (var traitId in Fields.Get<TraitsComponent>().TraitIds)
                world.TraitProvider.GetTrait(traitId)?.ExecuteInit(this, world);
        }

        public void Update(WorldObject world)
        {
            if (world.TraitProvider == null) return;
            foreach (var traitId in Fields.Get<TraitsComponent>().TraitIds)
                world.TraitProvider.GetTrait(traitId)?.ExecuteUpdate(this, world);
        }

        public void Dispose(WorldObject world)
        {
            if (world.TraitProvider == null) return;
            foreach (var traitId in Fields.Get<TraitsComponent>().TraitIds)
                world.TraitProvider.GetTrait(traitId)?.ExecuteDispose(this, world);
        }
    }
}