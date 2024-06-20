using System;

public static class DotsObjectEvents
{
    public static event Action<DotsGameObject> onHit;
    public static event Action<DotsGameObject> onCleared;
   


    public static void NotifyCleared(DotsGameObject instance)
    {
       
        onCleared?.Invoke(instance);
    }

    public static void NotifyHit(DotsGameObject instance)
    {

        onHit?.Invoke(instance);
    }
}