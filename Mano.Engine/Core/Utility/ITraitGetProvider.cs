namespace mano.Engine
{
    public interface ITraitProvider
    {
        ITrait? GetTrait(string traitId);
    }
    public interface ITrait
    {
        void ExecuteInit(Object target, WorldObject world);
        void ExecuteUpdate(Object target, WorldObject world);
        void ExecuteDispose(Object target, WorldObject world);
    }
}