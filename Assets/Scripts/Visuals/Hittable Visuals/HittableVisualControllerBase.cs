using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;

/// <summary>
/// Represents a visual controller base that controlls the visuals of a hittable Dots game object
/// </summary>
public class HittableVisualControllerBase : IHittableVisualController
{

    private IHittableVisuals visuals;
    private IHittable hittable;

    public void Init(IHittable hittable, IHittableVisuals visuals)
    {
        this.visuals = visuals;
        this.hittable = hittable;
        if(hittable is AnchorDot)
        {
            Debug.Log("HELLLOOOO" + visuals);
            
        }
    }


   


    public IEnumerator DoHitAnimation(HitType hitType)
    {
        DotsGameObject dotsGameObject = (DotsGameObject)hittable;
        yield return dotsGameObject.transform.DOScale(Vector2.zero, visuals.ClearDuration);
    }

    public virtual IEnumerator DoClearAnimation()
    {
        DotsGameObject dotsGameObject = (DotsGameObject)hittable;
        yield return dotsGameObject.transform.DOScale(Vector2.zero, visuals.ClearDuration);
       
    }

   
}
