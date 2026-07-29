namespace mano;

public class ContainerObject : ItemObject
{
    public ContainerTrait Container { get; set; } = new();
}