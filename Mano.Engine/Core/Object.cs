using System.Collections.Generic;

namespace mano.Engine
{
    public class Object
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public List<string> Traits { get; set; } = new List<Trait>();

        public virtual void Init(WorldObject worldObject)
        {
            foreach (var trait in Traits)
            {
                trait.ExecuteInit(this, worldObject);
            }
        }

        public virtual void Update(WorldObject worldObject)
        {
            foreach (var trait in Traits)
            {
                trait.ExecuteUpdate(this, worldObject);
            }
        }

        public virtual void Dispose(WorldObject world)
        {
            foreach (var trait in Traits)
            {
                trait.ExecuteDispose(this, world);
            }
            TraitIds.Clear();
        }
    }
}