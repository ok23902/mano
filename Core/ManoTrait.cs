namespace mano;

public abstract class ManoTrait
{
    public static Type? GetTargetType(Type traitType)
    {
        var t = traitType.BaseType;
        while (t != null)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ManoTrait<>))
                return t.GetGenericArguments()[0];
            t = t.BaseType;
        }
        return null;
    }
}

public abstract class ManoTrait<TTarget> : ManoTrait
    where TTarget : ManoObject
{
}