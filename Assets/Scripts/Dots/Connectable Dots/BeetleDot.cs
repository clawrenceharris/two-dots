using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class BeetleDot : ColorableDot, IDirectional, IPreviewable, IMulticolored
{
    public override DotType DotType => DotType.BeetleDot;
    public override int HitsToClear => 3;
    public override DotColor Color { get => colors[Mathf.Clamp(hitCount, 0, HitsToClear - 1)]; }

    public override DotsObjectData ReplacementDot
    {
        get
        {
            return new(JSONLevelLoader.ToJsonDotType(DotType.Bomb))
            {
                col = Column,
                row = Row
            };

        }
    }
    public HitType PreviewHitType { get; private set; }
    HitType IPreviewable.PreviewHitType => PreviewHitType;

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

        hitCount++;
        WasHit = true;
        
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
        PreviewHitType = hitType;
        yield return visualController.PreviewHit(hitType);   
    }

    
}
