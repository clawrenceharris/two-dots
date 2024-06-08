using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class BeetleDot : ColorableDot, IDirectional, IPreviewable, IMulticolored
{
    public override DotType DotType => DotType.BeetleDot;
    public override DotColor Color { get => colors[HitCount]; }
    public override int HitsToClear => 3;
    private int directionX;
    private int directionY;
    public int DirectionX { get => directionX; set => directionX = value; }
    public int DirectionY { get => directionY; set => directionY = value; }
    private DotColor[] colors;
    public DotColor[] Colors { get => colors; set => colors = value; }

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

    public BeetleDotVisualController VisualController
    {
        get
        {
            if (visualController is BeetleDotVisualController beetleDotVisualController)
            {
                return beetleDotVisualController;
            }
            throw new InvalidCastException("Unable to cast base visualController to BeetleDotVisualController");

        }
    }


    public override IEnumerator Clear()
    {
        
        NotifyDotCleared();
        DotController.DoBombDot(this);
        yield return null;
    }


    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        yield return DoVisualHit(hitType);

        yield return base.Hit(hitType);
    }

    public override void InitDisplayController()
    {
        visualController = new BeetleDotVisualController();
        visualController.Init(this);
    }

    public IEnumerator ChangeDirection(int directionX, int directionY)
    {
        DirectionX = directionX;
        DirectionY = directionY;

        yield return VisualController.RotateCo();
    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        HitType = hitType;
        yield return StartCoroutine(VisualController.PreviewHit(hitType));   
    }

    
}
