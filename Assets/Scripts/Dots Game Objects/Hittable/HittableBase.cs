using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableBase : IHittable
{
    public HitType HitType { get; set; }

    public bool WasHit { get; set; }
    public int HitCount { get; set; }
    public IHittable hittable;
    public int HitsToClear { get => hittable.HitsToClear; }
    public static event Action<IHittable> onCleared;
    public static event Action<IHittable> onHit;

    public Dictionary<HitType, IHitRule> HitRules { get => hittable.HitRules; }
    public DotsGameObject DotsGameObject => (DotsGameObject)hittable;
    public IHittableVisualController VisualController => DotsGameObject.GetVisualController<IHittableVisualController>();
   
    public void Init(IHittable hittable)
    {
        this.hittable = hittable;
    }


    public virtual IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        onHit?.Invoke(hittable);

        HitType = hitType;
        WasHit = true;
       

        onHitComplete?.Invoke();
        yield return VisualController.DoHitAnimation(hitType);
    }



    public IEnumerator Clear()
    {
        onCleared?.Invoke(hittable);
        yield return VisualController.DoClearAnimation();

    }

}
