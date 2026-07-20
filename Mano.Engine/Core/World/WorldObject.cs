using System.Collections.Generic;
using System.Linq;

namespace mano.Engine
{

    public class WorldObject : Object
    {
        public ITraitProvider? TraitProvider { get; set; }
        public DataContainer Static { get; set; } = new DataContainer();
        public DataContainer Dynamic { get; set; } = new DataContainer();

        public new WorldObject Clone()
        {
            var newWorld = new WorldObject
            {
                Id = this.Id,
                Fields = this.Fields.Clone(),
                Static = this.Static.Clone(),
                Dynamic = this.Dynamic.Clone()
            };
            return newWorld;
        }

        public class DataContainer
        {
            public Dictionary<string, Object> Character { get; set; } = new Dictionary<string, Object>();
            public Dictionary<string, Object> Item { get; set; } = new Dictionary<string, Object>();
            public Dictionary<string, Object> Room { get; set; } = new Dictionary<string, Object>();
            public Dictionary<string, Object> Rule { get; set; } = new Dictionary<string, Object>();

            public IEnumerable<Object> GetAllObjects()
            {
                return Character.Values
                    .Concat(Item.Values)
                    .Concat(Room.Values)
                    .Concat(Rule.Values);
            }

            public void Clear()
            {
                Character.Clear();
                Item.Clear();
                Room.Clear();
                Rule.Clear();
            }

            public DataContainer Clone()
            {
                var newContainer = new DataContainer();
                foreach (var kv in Character) newContainer.Character[kv.Key] = kv.Value.Clone();
                foreach (var kv in Item)      newContainer.Item[kv.Key]      = kv.Value.Clone();
                foreach (var kv in Room)      newContainer.Room[kv.Key]      = kv.Value.Clone();
                foreach (var kv in Rule)      newContainer.Rule[kv.Key]      = kv.Value.Clone();
                return newContainer;
            }
        }
    }
}