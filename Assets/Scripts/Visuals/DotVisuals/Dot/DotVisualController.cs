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

   

    public override void Init(DotsGameObject dotsGameObject)
    {
        Animator = dotsGameObject.GetComponent<DotsAnimator>();
        hittableVisualController.Init(this);
        SetUp();

    }

     public IEnumerator Clear(float duration)
    {
        yield return hittableVisualController.Clear(duration);
    }

    public IEnumerator Hit()
    {
        yield return hittableVisualController.Hit();
    }
    public IEnumerator Pulse()
    {
        throw new NotImplementedException();
    }

    
}
