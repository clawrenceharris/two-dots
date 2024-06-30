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



    public virtual void Pulse()
    {
        VisualController.Pulse();
    }

    public virtual IEnumerator Hit(HitType hitType)
    {
        DotsObjectEvents.NotifyHit(this);

        HitType = hitType;

        if (hitType == HitType.BombExplosion) {
            yield return VisualController.DoBombHit();

        }
        
        yield return VisualController.DoHitAnimation(hitType);
    }


    public virtual void UndoHit()
    {
        HitType = HitType.None;
    }

    protected IEnumerator Clear(float clearTime, Action<IHittable> onComplete)
    {
        DotsObjectEvents.NotifyCleared(this, clearTime);
        yield return VisualController.DoClearAnimation();
        onComplete?.Invoke(this);
    }

    public virtual IEnumerator Clear(Action<IHittable> onComplete)
    {
        yield return Clear(0f, onComplete);
    }


    public IEnumerator BombHit(Action onComplete)
    {
        HitType = HitType.BombExplosion;
        yield return VisualController.DoBombHit();
        onComplete?.Invoke();
    }
}
