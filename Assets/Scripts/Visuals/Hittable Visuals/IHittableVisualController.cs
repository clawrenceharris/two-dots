using UnityEngine;
using static Type;
using UnityEngine.U2D;
using System.Collections;
public interface IHittableVisualController
{

    public IEnumerator DoBombHit();


    public IEnumerator DoHitAnimation(HitType hitType);

    public IEnumerator DoClearAnimation();
   
}