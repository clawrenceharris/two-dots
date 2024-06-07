using System.Collections;
using UnityEngine;
using static Type;
using System;
using DG.Tweening;
using Color = UnityEngine.Color;
using System.Collections.Generic;

public abstract class Dot : MonoBehaviour, IHittable
{




    public static event Action<Dot> onDotCleared;
    public static event Action<Dot> onDotHit;
    public static event Action<Dot> onBombActivated;
    public static event Action<Dot> onBombDeactivated;

    private int row;
    private int column;
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }
    

    public abstract DotType DotType { get; }
    public bool IsBomb { get; protected set; }
    public IDotVisualController visualController;
    public int HitCount { get; protected set; }
    public abstract int HitsToClear { get; }
    public abstract Dictionary<HitType, IHitRule> HitRules { get; }


    public HitType HitType { get; protected set; }
    HitType IHittable.HitType => HitType;


    public virtual IEnumerator Clear()
    {
        yield return visualController.Clear();
        
        NotifyDotCleared();
    }


    public virtual void Init(int column, int row)
    {
        this.column = column;
        this.row = row;
        InitDisplayController();
    }

    public abstract void InitDisplayController();

    public virtual IEnumerator Hit(HitType hitType)
    {
        HitType = hitType;
        onDotHit?.Invoke(this);
        yield return null;

        
    }

    public virtual IEnumerator DoVisualHit(HitType hitType)
    {
        if(HitCount > HitsToClear)
        {
            yield break;
        }

        if (hitType == HitType.BombExplosion)
        {
            yield return visualController.BombHit();

        }

        yield return visualController.Hit(hitType);
    }

    public void Debug()
    {
        UnityEngine.Debug.Log("Dot: " + name);
        Debug(Color.black);
    }

    public void Debug(Color color)
    {
        visualController.SetColor(color);
    }


    

    protected virtual void NotifyDotCleared()
    {
        onDotCleared?.Invoke(this);
    }



    public virtual void Pulse()
    {
        //visualController.Pulse();
    }

}
