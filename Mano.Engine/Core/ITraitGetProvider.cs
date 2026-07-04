namespace mano.Engine
{
    public interface ITraitProvider
    {
        Trait? GetTrait(string traitId);
    }
}