using System.Collections.Generic;

namespace mano.Engine
{
    public class Object
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public List<string> Traits { get; set; } = new List<string>();
        public Dictionary<string, object> Fields { get; set; } = new Dictionary<string, object>();

        public Object Clone()
        {
            return new Object
            {
                Id = this.Id,
                Name = this.Name,
                Traits = new List<string>(this.Traits),
                Fields = new Dictionary<string, object>(this.Fields)
            };
        }

        public void Init(WorldObject world)
        {
            if (world.TraitProvider == null) return;
            foreach (var traitId in Traits)
            {
                world.TraitProvider.GetTrait(traitId)?.ExecuteInit(this, world);
            }
        }

        public void Update(WorldObject world)
        {
            if (world.TraitProvider == null) return;
            foreach (var traitId in Traits)
            {
                world.TraitProvider.GetTrait(traitId)?.ExecuteUpdate(this, world);
            }
        }

        public void Dispose(WorldObject world)
        {
            if (world.TraitProvider == null) return;
            foreach (var traitId in Traits)
            {
                world.TraitProvider.GetTrait(traitId)?.ExecuteDispose(this, world);
            }
        }
    }
}