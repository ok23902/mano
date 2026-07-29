namespace mano
{
    public class WorldObject : ManoObject
    {
        public WorldObject()
        {
            AttachTrait(TraitRegistry.Get<WorldTrait>());
        }

        public int TimeOfDay { get; set; } = 8;
        public int DayCount { get; set; } = 1;

        public List<RoomObject> Rooms { get; set; } = new();

        public string status { get; set; } = string.Empty;
    }
}