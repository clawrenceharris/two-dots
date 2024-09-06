using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;


public abstract class DotVisualController : 
VisualController, 
IDotVisualController, 
IHittableVisualController
{
    private readonly HittableVisualController hittableVisualController = new();

    /// <summary>
    /// Retrieves the <see cref="DotsAnimationComponent"/> attached to the 
    /// <see cref="DotsGameObject"/> that this visual controller manages.
    /// </summary>
    /// <typeparam name="T">A type that inherits from <see cref="DotsAnimationComponent"/></typeparam>
    /// <returns>An animation component of type <typeparamref name="T"/>.</returns>
    public T GetAnimationComponent<T>(AnimationLayer layer) where T : DotsAnimationComponent{
        return animator.GetAnimationComponent<T>(layer);
    }
    public override void Init(DotsGameObject dotsGameObject)
    {
        animator = dotsGameObject.GetComponent<DotsAnimator>();
        hittableVisualController.Init(this);
        SetUp();

    }

     public virtual IEnumerator Clear(float duration)
    {
        yield return hittableVisualController.Clear(duration);
    }

    public virtual IEnumerator Hit()
    {
        yield return hittableVisualController.Hit();
    }
    public IEnumerator Pulse()
    {
        throw new NotImplementedException();
    }

    
}
