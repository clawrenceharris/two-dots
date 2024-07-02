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
    
    public new DotVisualController VisualController => GetVisualController<DotVisualController>();
   
    public abstract DotType DotType { get; }

    public HitType HitType { get; protected set; }

    public int HitCount { get; set; }


    public  abstract int HitsToClear { get; }
    public bool WasHit { get; protected set; }

    public virtual void Pulse()
    {
        VisualController.Pulse();
    }

    public abstract void Hit(HitType hitType);
   

    public virtual IEnumerator Hit(HitType hitType, Action onHitChanged = null)
    {
        DotsObjectEvents.NotifyHit(this);

        HitType = hitType;
        WasHit = true;
        Hit(hitType);
        if (hitType == HitType.BombExplosion) {
            yield return VisualController.DoBombHit();

        }

        onHitChanged?.Invoke();
        yield return VisualController.DoHitAnimation(hitType);
    }


    public virtual void UndoHit()
    {
        HitType = HitType.None;
    }

    public IEnumerator Clear()
    {
        DotsObjectEvents.NotifyCleared(this);
        yield return VisualController.DoClearAnimation();

    }

   

    public IEnumerator BombHit(Action onComplete)
    {
        HitType = HitType.BombExplosion;
        yield return VisualController.DoBombHit();
        onComplete?.Invoke();
    }
}
