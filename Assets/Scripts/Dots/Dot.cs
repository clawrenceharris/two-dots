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
    //public static event Action<Dot> onBombActivate;
    public virtual DotsObjectData ReplacementDot => null;
    private int row;
    private int column;
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }

    public T GetVisualController<T>() where T : class
    {
        if (visualController is T controller)
        {
            return controller;
        }
        throw new InvalidCastException($"Unable to cast base visualController to {typeof(T).Name}");
    }

    public abstract Dictionary<HitType, IHitRule> HitRules { get; }

    public IDotVisualController visualController;

    public abstract DotType DotType { get; }
    public bool IsBomb { get; protected set; }



    protected int hitCount;
    public int HitCount { get => hitCount; set => hitCount = value; }

    public abstract int HitsToClear { get; }


    public HitType HitType { get; protected set; }




    public virtual void Init(int column, int row)
    {
        this.column = column;
        this.row = row;
        InitDisplayController();
    }

    public abstract void InitDisplayController();



    protected virtual void NotifyDotCleared()
    {
        onDotCleared?.Invoke(this);
    }


    public virtual IEnumerator Clear()
    {
        NotifyDotCleared();

        yield return visualController.Clear();

    }
    public virtual IEnumerator Hit(HitType hitType)
    {
        HitType = hitType;
        onDotHit?.Invoke(this);
        yield return DoVisualHit(hitType);


    }
    protected void NotifyDotHit()
    {
        onDotHit?.Invoke(this);
    }
    public virtual IEnumerator DoVisualHit(HitType hitType)
    {
        yield return visualController.Hit(hitType);
        if (this is IPreviewable previewable)
        {
            CoroutineHandler.StartStaticCoroutine(previewable.PreviewHit(hitType));
        }
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


    

    //protected void NotifyBombActive()
    //{
    //    onBombActivate?.Invoke(this);
    //}

    public virtual void Pulse()
    {
        //visualController.Pulse();
    }

}
