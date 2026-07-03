public class CharacterAIObject : CharacterObject
{
    public string Goal = "";

    public List<Information> FocusedInformation = new();

    public int Sleepness = 0;
    public int Hungry = 0;
    public int Thirsty= 0;
    public int Boredom= 0;
    public int Bio = 0;
}