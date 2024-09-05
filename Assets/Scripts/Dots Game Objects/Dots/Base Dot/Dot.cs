using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;
using System.Collections.Generic;

public abstract class Dot : DotsGameObject, IHittable
{

    
    public new DotVisualController VisualController => GetVisualController<DotVisualController>();
    public abstract DotType DotType { get; }

    private readonly HittableBase hittable = new();

    public abstract IHitRule HitRule { get; }

    public HitType HitType { get => hittable.HitType; protected set => hittable.HitType = value; }

    public int HitCount { get => hittable.HitCount; set => hittable.HitCount = value; }


    public  abstract int HitsToClear { get; }
    public bool WasHit { get => hittable.WasHit;  set => hittable.WasHit = value; }

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        hittable.Init(this);
    }


   public virtual void Clear()
    { 
        hittable.Clear();
    }

    public void Clear(float duration)
    {
        hittable.Clear(duration);

    }
    public virtual void Hit(HitType hitType, Action onHitComplete = null)
    {

        Hit(hitType);
        hittable.Hit(hitType, onHitComplete);
    }


   
    
    public abstract void Hit(HitType hitType);

    public virtual void Pulse()
    {
        VisualController.Pulse();
    }


}
