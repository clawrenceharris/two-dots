using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Animations;

/// <summary>
/// Represents a visual controller base that controlls the visuals of a hittable Dots game object
/// </summary>
public class HittableVisualController : IHittableVisualController
{
    private VisualController visualController;
    
    public void Init(VisualController visualController)
    {
        this.visualController = visualController;
    }

   
    
    public IEnumerator Hit(){
        
        yield return visualController.Animate(new HitAnimation());

    }

    
    public IEnumerator Clear(float duration){
        
        IClearable clearable = visualController.Animator.GetAnimatableComponent<IClearable>();
        clearable.Settings.Duration = duration;
        yield return visualController.Animate(new ClearAnimation());
    }

    
    

}