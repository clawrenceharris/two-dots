using System.Collections;


public interface IHittableVisualController
{
   
    

    IEnumerator DoHitAnimation(HitType hitType);
    IEnumerator DoClearAnimation();
}