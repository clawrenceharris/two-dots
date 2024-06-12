using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static Type;
public class BeetleDot : ColorableDot, IDirectional, IPreviewable, IMulticolored
{
    public override DotType DotType => DotType.BeetleDot;
    public override DotColor Color { get => colors[HitCount]; }
    public override int HitsToClear => 3;

    public PreviewHitType PreviewHitType { get; private set; }
    PreviewHitType IPreviewable.PreviewHitType => PreviewHitType;

    private int directionX;
    private int directionY;
    public int DirectionX { get => directionX; set => directionX = value; }
    public int DirectionY { get => directionY; set => directionY = value; }
    private DotColor[] colors;
    public DotColor[] Colors { get => colors; set => colors = value; }
    public bool WasHit { get; private set; }
    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new(base.HitRules)
            {
                {
                    HitType.BeetleDot, new HitByConnectionRule()
                },
            };
        }
    }

    public BeetleDotVisualController VisualController => GetVisualController<BeetleDotVisualController>();
   


    public override IEnumerator Clear()
    {
        StartCoroutine(base.Clear());
        NotifyBombActive();
        yield return null;
    }

    public IEnumerator DoSwap(Dot dotToSwap, Action callback)
    {
        

        if (!WasHit)
        {
            yield return VisualController.DoSwap(dotToSwap);
           
            WasHit = false;
            callback?.Invoke();
        }     
    }

    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        WasHit = true;
        yield return DoVisualHit(hitType);

        yield return base.Hit(hitType);
    }

    public override void InitDisplayController()
    {
        base.visualController = new BeetleDotVisualController();
        base.visualController.Init(this);
    }

    public IEnumerator ChangeDirection(int directionX, int directionY)
    {
        DirectionX = directionX;
        DirectionY = directionY;

        yield return VisualController.RotateCo();
    }

    public IEnumerator PreviewHit(PreviewHitType hitType)
    {
        PreviewHitType = hitType;
        yield return visualController.PreviewHit(hitType);   
    }

    
}
