namespace mano
{
    public class BioTrait : ManoTrait<CharacterAIObject>
    {
        public void OnUpdate(CharacterAIObject character)
        {
            character.Bio += 1;
        }
    }
}