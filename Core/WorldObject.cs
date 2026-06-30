public class WorldObject : Object
    {
        public Dictionary<string, Object> Character { get; set; } = new Dictionary<string, Object>();
        public Dictionary<string, Object> Item { get; set; } = new Dictionary<string, Object>();
        public Dictionary<string, Object> Room { get; set; } = new Dictionary<string, Object>();
        public Dictionary<string, Object> Rule { get; set; } = new Dictionary<string, Object>();

        Traits = new List<string> { "WorldTrait" };
        
        public override void Dispose()
        {
            Character.Clear();
            Item.Clear();
            Room.Clear();
            Rule.Clear();
            base.Dispose();
        }
    }