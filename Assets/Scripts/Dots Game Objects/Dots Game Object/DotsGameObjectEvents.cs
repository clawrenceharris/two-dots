using System;


public class DotsGameObjectEvents{
    public static event Action<DotsGameObject> onCleared;
    public static event Action<DotsGameObject> onHit;

    public static void NotifyCleared(DotsGameObject dotsGameObject){
        onCleared?.Invoke(dotsGameObject);
    }
    public static void NotifyHit(DotsGameObject dotsGameObject){
        
        onHit?.Invoke(dotsGameObject);
    }


}