using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class HittableBase : IHittable
{
    public HitType HitType { get; private set; }

    public bool WasHit { get; set; }
    public int HitCount { get; set; }
    public IHittable hittable;
    public int HitsToClear { get => hittable.HitsToClear; }

    public Dictionary<HitType, IHitRule> HitRules { get => hittable.HitRules; }
    public DotsGameObject DotsGameObject => (DotsGameObject)hittable;
    public IHittableVisualController VisualController => DotsGameObject.GetVisualController<IHittableVisualController>();
   
    public void Init(IHittable hittable)
    {
        this.hittable = hittable;
    }


    public virtual IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        DotsObjectEvents.NotifyHit(DotsGameObject);

        HitType = hitType;
        WasHit = true;
        if (hitType == HitType.BombExplosion)
        {
            yield return VisualController.DoBombHit();

        }

        onHitComplete?.Invoke();
        yield return VisualController.DoHitAnimation(hitType);
    }



    public IEnumerator Clear()
    {
        DotsObjectEvents.NotifyCleared(DotsGameObject);
        yield return VisualController.DoClearAnimation();

    }

}
