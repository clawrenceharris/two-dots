using System.Collections;


public interface IHittableVisualController
{
   
    

    IEnumerator Hit(HitType hitType);
    IEnumerator Clear(float duration);
    
}