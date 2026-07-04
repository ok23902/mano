using System.Collections.Generic;
using mano.Engine;

namespace mano.Runtime
{
    public class TraitRegistry : ITraitProvider
    {
        private readonly Dictionary<string, Trait> _traits = new();

        public void Register(string id, Trait trait)
        {
            _traits[id] = trait;
        }

        public Trait? GetTrait(string traitId)
        {
            if (_traits.TryGetValue(traitId, out var trait))
            {
                return trait;
            }
            return null;
        }
    }
}