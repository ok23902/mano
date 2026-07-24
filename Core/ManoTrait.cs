namespace mano
{
    public abstract class ManoTrait
    {
    }

    public abstract class ManoTrait<TTarget> : ManoTrait where TTarget : ManoObject
    {
    }
}