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
   

    public  IHitRule HitRule { get => hittable.HitRule; }
    private DotsGameObject DotsGameObject => (DotsGameObject)hittable;
    public IHittableVisualController VisualController => DotsGameObject.GetVisualController<IHittableVisualController>();
   
    public void Init(IHittable hittable)
    {
        this.hittable = hittable;
    }


    public virtual IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        DotsGameObjectEvents.NotifyHit(DotsGameObject);

        HitType = hitType;
        WasHit = true;
        
        if(hittable.HitCount <= hittable.HitsToClear){
            onHitComplete?.Invoke();

        }
        yield return VisualController.Hit(hitType);
    }



    public IEnumerator Clear()
    {  
        IVisualController visualController = DotsGameObject.GetVisualController<IVisualController>();
        float duration = visualController.GetVisuals<IHittableVisuals>().ClearDuration;
        yield return Clear(duration);    
    }
    
    public IEnumerator Clear(float duration){
        DotsGameObjectEvents.NotifyCleared(DotsGameObject);
        yield return VisualController.Clear(duration);    
    }
    
    


}
