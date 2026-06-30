using System.Collections.Generic;

namespace mano
{
    public class Object
    {
        public string Id { get; set; } = string.Empty;
        public List<string> Traits { get; set; } = new List<string>();

        public virtual void Init(WorldObject world, ITraitRegistry registry)
        {
            foreach (var traitName in Traits)
            {
                registry.GetTrait(traitName)?.ExecuteInit(this, world);
            }
        }

        public virtual void Update(WorldObject world, ITraitRegistry registry)
        {
            foreach (var traitId in Traits)
            {
                registry.GetTrait(traitId)?.ExecuteUpdate(this, world);
            }
        }

        public virtual void Dispose(WorldObject world, ITraitRegistry registry)
        {
            foreach (var traitName in Traits)
            {
                registry.GetTrait(traitName)?.ExecuteDispose(this, world);
            }
            Traits.Clear();
        }
    }

    public interface ITraitRegistry
    {
        Trait GetTrait(string name);
    }
}