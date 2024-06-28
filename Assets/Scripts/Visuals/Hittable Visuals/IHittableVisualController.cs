using UnityEngine;
using static Type;
using UnityEngine.U2D;
using System.Collections;
public interface IHittableVisualController
{

    public IEnumerator BombHit();


    public IEnumerator HitAnimation(HitType hitType);

    public IEnumerator ClearAnimation();
   
}