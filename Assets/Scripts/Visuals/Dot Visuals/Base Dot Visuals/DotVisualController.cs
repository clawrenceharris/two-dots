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

    

    public virtual IEnumerator Clear()
    {
        yield return hittableVisualController.Clear();
    }

    public virtual IEnumerator Hit(HitType hitType)
    {
        yield return hittableVisualController.Hit(hitType);
    }

    public IEnumerator Pulse()
    {
        throw new NotImplementedException();
    }

   
}
