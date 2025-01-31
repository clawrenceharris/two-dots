using System;
using System.Collections;
using System.Collections.Generic;
using Animations;
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
    private IHittableVisualController VisualController => DotsGameObject.GetVisualController<IHittableVisualController>();

    public void Init(IHittable hittable)
    {
        this.hittable = hittable;
    }


    public IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        DotsGameObjectEvents.NotifyHit(DotsGameObject);

        HitType = hitType;
        WasHit = true;
        
        if(hittable.HitCount <= hittable.HitsToClear){
            onHitComplete?.Invoke();

        }

        yield return VisualController.Hit();
    }



    public IEnumerator Clear()
    {   
        
        IClearable clearable = DotsGameObject.VisualController?.GetAnimatableComponent<IClearable>();
        if(clearable != null)
            yield return Clear(clearable.Settings.Duration);  
        else{
            Clear(0);
        }  
    }
    
    public IEnumerator Clear(float duration){

        DotsGameObjectEvents.NotifyCleared(DotsGameObject);
        yield return VisualController.Clear(duration);    
    }
    
    


}
