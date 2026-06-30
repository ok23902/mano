public abstract class WorldTrait : Trait
    {
        public override OnInit(Object targetObject, WorldObject worldObject) { }
        {
            foreach (character in targetObject.Characters)
            targetObject.character.Init(worldObject.Static);
        }
    }