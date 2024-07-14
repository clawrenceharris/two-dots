using System.Collections;
using static Type;
public interface IHittableVisualController
{
   
    

    IEnumerator DoHitAnimation(HitType hitType);
    IEnumerator DoClearAnimation();
}