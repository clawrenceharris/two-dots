using System.Collections;
using static Type;
public interface IHittableVisualController
{
    /// <summary>
    /// Starts the coroutine to visually
    /// indicate that a hittable has been hit by a bomb 
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator DoBombHit();

    IEnumerator DoHitAnimation(HitType hitType);
    IEnumerator DoClearAnimation();
}