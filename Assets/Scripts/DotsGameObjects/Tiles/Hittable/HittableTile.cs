using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class HittableTile : Tile, IHittable
{

    private readonly HittableBase hittable = new();

    public abstract IHitRule HitRule { get; }

    public HitType HitType { get => hittable.HitType; protected set => hittable.HitType = value; }

    public int HitCount { get => hittable.HitCount; set => hittable.HitCount = value; }


    public  abstract int HitsToClear { get; }
    public bool WasHit { get => hittable.WasHit; set => hittable.WasHit = value; }

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        hittable.Init(this);
    }


    public virtual IEnumerator Clear()
    {
        
        yield return hittable.Clear();

    }

    public virtual IEnumerator Clear(float duration)
    {
        yield return hittable.Clear(duration);

    }
    public virtual IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        Hit(hitType);
        yield return hittable.Hit(hitType, onHitComplete);
    }


   
    
    public abstract void Hit(HitType hitType);

    

}
