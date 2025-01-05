using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;


public abstract class DotVisualController : 
VisualController, 
IDotVisualController, 
IHittableVisualController,
IAnimatableVisualController
{
    
    private readonly HittableVisualController hittableVisualController = new();
    private readonly AnimatableVisualController animatableVisualController = new();
   
    public override void Init(DotsGameObject dotsGameObject)
    {
        hittableVisualController.Init(this);
        animatableVisualController.Init(this);
        SetUp();

    }

    public virtual IEnumerator Hit()
    {
        yield return hittableVisualController.Hit();
    }


    public virtual IEnumerator Clear(float duration)
    {
        yield return hittableVisualController.Clear(duration);
    }

    

    public IEnumerator Pulse()
    {
        throw new NotImplementedException();
    }
    public IEnumerator Move(List<Vector3> positions){
        yield return animatableVisualController.Move(positions);

    }

    public IEnumerator Move(Vector3 position){
        List<Vector3> positions = new(){position};
        yield return animatableVisualController.Move(positions);

    }
    

    public IEnumerator Rotate(Vector3 targetRotation)
    {
        yield return animatableVisualController.Rotate(targetRotation);
    }
    


    public IEnumerator Drop(float targetY)
    {
        yield return animatableVisualController.Drop(targetY);
    }

    public IEnumerator Swap(Vector3 targetPosition)
    {
        yield return animatableVisualController.Swap(targetPosition);
    }
}
