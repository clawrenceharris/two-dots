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

    public static event Action<Dot> onDotCleared;
    public static event Action<Dot> onDotHit;

    protected int hitCount;
    public int HitCount { get => hitCount; set => hitCount = value; }

    public abstract int HitsToClear { get; }

    public DotVisualController VisualController => GetVisualController<DotVisualController>();


    public HitType HitType { get; protected set; }



    public virtual IEnumerator Hit(HitType hitType)
    {
        HitType = hitType;
        DotsObjectEvents.NotifyHit(this);
        yield return VisualController.Hit(hitType);
    }
   
    public abstract DotType DotType { get; }


    public IEnumerator Clear()
    {
        DotsObjectEvents.NotifyCleared(this);
        yield return VisualController.Clear();
    }



    public virtual void Pulse()
    {
        VisualController.Pulse();
    }

}
