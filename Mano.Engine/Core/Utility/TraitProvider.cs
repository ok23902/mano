using System.Collections.Generic;

namespace mano.Engine
{
    public class TraitProvider : ITraitProvider
    {
        private readonly Dictionary<string, ITrait> _traits = new Dictionary<string, ITrait>();

        public void RegisterTrait(string id, ITrait trait)
        {
            if (trait == null) return;
            _traits[id] = trait;
        }

        public ITrait? GetTrait(string traitId)
        {
            if (string.IsNullOrEmpty(traitId)) return null;
            
            _traits.TryGetValue(traitId, out var trait);
            return trait;
        }
    }
}