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

    public bool DidExecute { get; protected set; }
    public static event Action<Dot> onDotCleared;
    public static event Action<Dot> onDotHit;
    
    public DotVisualController VisualController => GetVisualController<DotVisualController>();
   
    public abstract DotType DotType { get; }

    public HitType HitType { get; protected set; }

    public int HitCount { get; set; }


    public  abstract int HitsToClear { get; }



    public virtual void Pulse()
    {
        VisualController.Pulse();
    }

    public virtual IEnumerator Hit(HitType hitType)
    {
        HitType = hitType;
        DotsObjectEvents.NotifyHit(this);
        yield return VisualController.Hit(hitType) ;
    }



    public virtual void UndoHit()
    {
        HitType = HitType.None;
    }

    public virtual IEnumerator Clear()
    {
        DotsObjectEvents.NotifyCleared(this);
        yield return VisualController.Clear();
    }

   
}
