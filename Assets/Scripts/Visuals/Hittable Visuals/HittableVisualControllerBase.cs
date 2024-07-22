using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Represents a visual controller base that controlls the visuals of a hittable Dots game object
/// </summary>
public class HittableVisualController : IHittableVisualController
{

    private IHittableVisuals visuals;
    private IHittable hittable;

    public void Init(IHittable hittable, IHittableVisuals visuals)
    {
        this.visuals = visuals;
        this.hittable = hittable;
    }


    public IEnumerator DoHitAnimation(HitType hitType)
    {
        yield return null;
    }

    public virtual IEnumerator DoClearAnimation()
    {
        DotsGameObject dotsGameObject = (DotsGameObject)hittable;
        yield return dotsGameObject.transform.DOScale(Vector2.zero, visuals.ClearDuration);
       
    }

   
}
