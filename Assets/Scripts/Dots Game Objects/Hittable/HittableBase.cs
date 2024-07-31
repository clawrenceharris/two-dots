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
    private DotsGameObject DotsGameObject => (DotsGameObject)hittable;
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
        if(hittable.HitCount <= hittable.HitsToClear){
            onHitComplete?.Invoke();

        }
        CoroutineHandler.StartStaticCoroutine(VisualController.Hit(hitType));
        yield return null;
    }



    public IEnumerator Clear()
    {  
        IVisualController visualController = DotsGameObject.GetVisualController<IVisualController>();
        float duration = visualController.GetVisuals<IHittableVisuals>().ClearDuration;
        yield return Clear(duration);    
    }

    public IEnumerator Clear(float duration){
        onCleared?.Invoke(hittable);
        yield return VisualController.Clear(duration);    
    }


}
