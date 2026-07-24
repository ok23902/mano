namespace mano
{
    public class CharacterObject : ManoObject
    {
        public override List<ManoTrait> Traits { get; set; } = new()
        {
            //TraitRegistry.Get<CharacterTrait>()
        };
        public string Personality { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Weight { get; set; } = 0f;

        public List<ActionComponent> Actions { get; set; } = new();
        public List<string> Inventory { get; set; } = new();
        public List<InformationComponent> Memory { get; set; } = new();
    }

    public class CharacterAIObject : CharacterObject
    {
        public override List<ManoTrait> Traits { get; set; } = new()
        {
            //TraitRegistry.Get<CharacterTrait>(),
            //TraitRegistry.Get<CharacterAITrait>()
        };
        public string Goal { get; set; } = string.Empty;
        public List<InformationComponent> FocusedInformation { get; set; } = new();

        public int Sleepiness { get; set; } = 0;
        public int Hungry { get; set; } = 0;
        public int Thirsty { get; set; } = 0;
        public int Boredom { get; set; } = 0;
        public int Bio { get; set; } = 0;
    }

    public class RoomObject : ManoObject
    {
        public override List<ManoTrait> Traits { get; set; } = new()
        {
            //TraitRegistry.Get<RoomTrait>()
        };
        public List<string> Characters { get; set; } = new();
        public List<string> Items { get; set; } = new();
        public List<string> Rules { get; set; } = new();
    }
}