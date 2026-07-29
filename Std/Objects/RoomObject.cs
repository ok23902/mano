namespace mano;
public class RoomObject : ManoObject
{
    public RoomObject()
    {
        AttachTrait(TraitRegistry.Get<RoomTrait>());
    }

    public string Name { get; set; } = string.Empty;
    public List<ManoObject> Characters { get; set; } = new();
    public List<ManoObject> Items { get; set; } = new();
}