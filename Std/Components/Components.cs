namespace mano
{
    public class TraitsComponent
    {
        public List<string> TraitIds { get; set; } = new List<string>();
    }
    public class NameComponent
    {
        public string Value { get; set; } = "";
    }

    public class ActionComponent
    {
        public string Command { get; set; } = "";
        public List<string> Args { get; set; } = new List<string>();
    }

    public class InformationComponent
    {
        public string Content { get; set; } = "";
        public Tick Tick { get; set; }
    }

    public class UtteranceComponent : InformationComponent
    {
        public string SpeakerId { get; set; } = "";
    }

    public class RuleComponent
    {
        public object? Value { get; set; }
    }

    public class PositionComponent
    {
        public Vector2 Value { get; set; } = new Vector2(0, 0);
    }

    public class TimeComponent
    {
        public Tick Value { get; set; }
    }
    
    public class PersonalityComponent { public string Value { get; set; } = ""; }
    public class StatusComponent { public string Value { get; set; } = "normal"; }
    public class ActionsComponent { public List<ActionComponent> Values { get; set; } = new(); }
    public class InventoryComponent { public List<string> ItemIds { get; set; } = new(); }
    public class RoomIdComponent { public string Value { get; set; } = "home"; }
    public class WeightComponent { public float Value { get; set; } = 50f; }
    
    public class GoalComponent { public string Value { get; set; } = ""; }
    public class FocusedInformationComponent { public List<InformationComponent> Values { get; set; } = new(); }
    public class BioStateComponent 
{ 
    public int Sleepiness { get; set; }
    public int Hungry { get; set; }
    public int Thirsty { get; set; }
    public int Boredom { get; set; }
    public int Bio { get; set; }
}
    
    public class RoomCharactersComponent { public List<string> CharacterIds { get; set; } = new(); }
    public class RoomItemsComponent { public List<string> ItemIds { get; set; } = new(); }
    public class RoomRulesComponent { public List<string> RuleIds { get; set; } = new(); }
    
    public class MemoryComponent 
    { 
        public List<InformationComponent> Values { get; set; } = new(); 
    }
}