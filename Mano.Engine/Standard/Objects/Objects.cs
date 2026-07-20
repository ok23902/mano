using System.Collections.Generic;

namespace mano.Engine
{
    public class CharacterObject : Object
    {
        public CharacterObject()
        {
            Fields.Get<PersonalityComponent>();
            Fields.Get<StatusComponent>();
            Fields.Get<ActionsComponent>();
            Fields.Get<InventoryComponent>();
            Fields.Get<MemoryComponent>();
            Fields.Get<RoomIdComponent>();
            Fields.Get<PositionComponent>();
            Fields.Get<WeightComponent>();
        }

        public string Personality 
        { 
            get => Fields.Get<PersonalityComponent>().Value; 
            set => Fields.Get<PersonalityComponent>().Value = value; 
        }
        public string Status 
        { 
            get => Fields.Get<StatusComponent>().Value; 
            set => Fields.Get<StatusComponent>().Value = value; 
        }
        public string RoomId 
        { 
            get => Fields.Get<RoomIdComponent>().Value; 
            set => Fields.Get<RoomIdComponent>().Value = value; 
        }
        public Vector2 Position 
        { 
            get => Fields.Get<PositionComponent>().Value; 
            set => Fields.Get<PositionComponent>().Value = value; 
        }
        public float Weight 
        { 
            get => Fields.Get<WeightComponent>().Value; 
            set => Fields.Get<WeightComponent>().Value = value; 
        }

        public List<ActionComponent> Actions => Fields.Get<ActionsComponent>().Values;
        public List<string> Inventory => Fields.Get<InventoryComponent>().ItemIds;
        public List<InformationComponent> Memory => Fields.Get<MemoryComponent>().Values;
    }

    public class CharacterAIObject : CharacterObject
    {
        public CharacterAIObject() : base()
        {
            Fields.Get<GoalComponent>();
            Fields.Get<FocusedInformationComponent>();
            Fields.Get<BioStateComponent>();
        }

        public string Goal 
        { 
            get => Fields.Get<GoalComponent>().Value; 
            set => Fields.Get<GoalComponent>().Value = value; 
        }
        
        public List<InformationComponent> FocusedInformation => Fields.Get<FocusedInformationComponent>().Values;

        public int Sleepness 
        { 
            get => Fields.Get<BioStateComponent>().Sleepiness; 
            set => Fields.Get<BioStateComponent>().Sleepiness = value; 
        }
        public int Hungry 
        { 
            get => Fields.Get<BioStateComponent>().Hungry; 
            set => Fields.Get<BioStateComponent>().Hungry = value; 
        }
        public int Thirsty 
        { 
            get => Fields.Get<BioStateComponent>().Thirsty; 
            set => Fields.Get<BioStateComponent>().Thirsty = value; 
        }
        public int Boredom 
        { 
            get => Fields.Get<BioStateComponent>().Boredom; 
            set => Fields.Get<BioStateComponent>().Boredom = value; 
        }
        public int Bio 
        { 
            get => Fields.Get<BioStateComponent>().Bio; 
            set => Fields.Get<BioStateComponent>().Bio = value; 
        }
    }

    public class RoomObject : Object
    {
        public RoomObject()
        {
            Fields.Get<RoomCharactersComponent>();
            Fields.Get<RoomItemsComponent>();
            Fields.Get<RoomRulesComponent>();
        }

        public List<string> Characters => Fields.Get<RoomCharactersComponent>().CharacterIds;
        public List<string> Items => Fields.Get<RoomItemsComponent>().ItemIds;
        public List<string> Rules => Fields.Get<RoomRulesComponent>().RuleIds;
    }
}