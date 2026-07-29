namespace mano
{
    public class CharacterTrait : ManoTrait<CharacterObject>
    {
        public void OnInit(CharacterObject character)
        {
            Console.WriteLine($"CharacterObject Initialized : name:{character.Name}");
            character.Up("OnUpCasted");
        }
        public void OnDownCast(CharacterObject character)
        {
            Console.WriteLine($"Personality: {character.Name} : {character.Personality}");
        }
    }
}