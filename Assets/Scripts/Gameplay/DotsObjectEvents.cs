using System;

public static class DotsObjectEvents
{
    public static event Action<DotsGameObject> onHit;
    public static event Action<DotsGameObject, float> onCleared;
   


    public static void NotifyCleared(DotsGameObject instance, float clearTime)
    {
       
        onCleared?.Invoke(instance, clearTime);
    }

    public static void NotifyHit(DotsGameObject instance)
    {

        onHit?.Invoke(instance);
    }
}