using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class BeetleDot : ColorableDot, IDirectional, IPreviewable
{
    public override DotType DotType => DotType.BeetleDot;

    public override int HitsToClear => 3;

   
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

    private BeetleDotVisualController VisualController
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
    private int directionX;
    private int directionY;
    public int DirectionX { get => directionX; set => directionX = value; }
    public int DirectionY { get => directionY; set => directionY = value; }

    public override IEnumerator Clear()
    {
        NotifyDotCleared();
        DotController.DoBombDot(this);
        yield return null;
    }


    public override IEnumerator Hit(HitType hitType)
    {
        
        if (hitType == HitType.BeetleDot || hitType == HitType.BombExplosion)
        {
            HitCount++;
        }
        
        yield return DoVisualHit(hitType);

        yield return base.Hit(hitType);
    }

    public override void InitDisplayController()
    {
        visualController = new BeetleDotVisualController();
        visualController.Init(this);
    }

    public IEnumerator AlternateDirection()
    {
        //Rotate 90 deg
        int temp = DirectionX;
        DirectionX = DirectionY;
        DirectionY = -temp;

        yield return VisualController.Rotate();
    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        yield return visualController.PreviewHit(hitType);
    }
}
