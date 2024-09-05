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
   
    public void Init(IHittable hittable)
    {
        this.hittable = hittable;
    }


    public virtual void Hit(HitType hitType, Action onHitComplete = null)
    {
        DotsGameObjectEvents.NotifyHit(DotsGameObject);

        HitType = hitType;
        WasHit = true;
        
        if(hittable.HitCount <= hittable.HitsToClear){
            onHitComplete?.Invoke();

        }
    }



    public void Clear()
    {  
        VisualController visualController = DotsGameObject.GetVisualController<VisualController>();
        float duration = visualController.GetVisuals<IHittableVisuals>().ClearDuration;
        Clear(duration);    
    }
    
    public void Clear(float duration){

        DotsGameObjectEvents.NotifyCleared(DotsGameObject);
        VisualController visualController = DotsGameObject.GetVisualController<VisualController>();
        CoroutineHandler.StartStaticCoroutine(visualController.Animate(new ClearAnimation(){
            Settings = new AnimationSettings(){
                Duration = duration
            }
        }));    
    }
    
    


}
