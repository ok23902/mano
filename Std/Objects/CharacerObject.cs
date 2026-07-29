namespace mano
{
    public class CharacterObject : ManoObject
    {
        public CharacterObject()
        {
            AttachTrait(TraitRegistry.Get<CharacterTrait>());
        }

        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Personality { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Weight { get; set; } = 0f;

        public List<ActionComponent> Actions { get; set; } = new();
        public List<string> Inventory { get; set; } = new();
        public List<InformationComponent> Memory { get; set; } = new();
    }

    public class CharacterAIObject : CharacterObject
    {
        public CharacterAIObject()
        {
            AttachTrait(TraitRegistry.Get<BioTrait>());
        }

        public string Goal { get; set; } = string.Empty;
        public List<InformationComponent> FocusedInformation { get; set; } = new();

        public int Sleepiness { get; set; } = 0;
        public int Hungry { get; set; } = 0;
        public int Thirsty { get; set; } = 0;
        public int Boredom { get; set; } = 0;
        public int Bio { get; set; } = 0;
    }

    public class PlayerObject : CharacterObject
    {
    }
}