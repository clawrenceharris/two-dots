using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public abstract class DotVisualController : VisualController, IDotVisualController, IHittableVisualController

{

    protected readonly HittableVisualController hittableVisualController = new();

    public override void Init(DotsGameObject dotsGameObject)
    {
        IHittable hittable = GetGameObject<IHittable>();
        IHittableVisuals visuals = GetVisuals<IHittableVisuals>();

        hittableVisualController.Init(hittable, visuals);
        SetUp();
    }

    

    public virtual IEnumerator DoClearAnimation()
    {
        yield return hittableVisualController.DoClearAnimation();
    }

    public virtual IEnumerator DoHitAnimation(HitType hitType)
    {
        yield return hittableVisualController.DoHitAnimation(hitType);
    }

    public IEnumerator Pulse()
    {
        throw new NotImplementedException();
    }

   
}
