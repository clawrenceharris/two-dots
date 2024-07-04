using System.Collections;
using UnityEngine;
using static Type;
using System;
using DG.Tweening;
using Color = UnityEngine.Color;
using System.Collections.Generic;

public abstract class Dot : DotsGameObject, IHittable
{

    public abstract Dictionary<HitType, IHitRule> HitRules { get; }
    
    public new DotVisualController VisualController => GetVisualController<DotVisualController>();
    private readonly HittableBase hittable = new();
    public abstract DotType DotType { get; }

    public HitType HitType { get => hittable.HitType; }

    public int HitCount { get => hittable.HitCount; set => hittable.HitCount = value; }


    public  abstract int HitsToClear { get; }
    public bool WasHit { get => hittable.WasHit; set => hittable.WasHit = value; }

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        hittable.Init(this);
    }

    public virtual void Pulse()
    {
        VisualController.Pulse();
    }

    public abstract void Hit(HitType hitType);
   

    public virtual IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        Hit(hitType);
        yield return hittable.Hit(hitType, onHitComplete);
    }


   
    public IEnumerator Clear()
    {
        yield return hittable.Clear();

    }

 
}
