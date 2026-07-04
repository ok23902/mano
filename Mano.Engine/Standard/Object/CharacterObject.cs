namespace mano.Engine
{
    public class CharacterObject : Object
    {
        public string Personality = "";
        public string Status = "normal";

        public List<Action> Actions = new();

        public List<ItemObject> Inventory = new();

        public List<Memory> Memory = new();

        public string RoomId = "home";

        public Vector2 Position = new Vector2(0, 0);

        public float Weight = 50;
    }
}