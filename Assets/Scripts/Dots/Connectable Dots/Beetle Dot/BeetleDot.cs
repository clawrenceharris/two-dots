using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class BeetleDot : ConnectableDot, IDirectional, IPreviewable, IMulticolorable
{
    public override DotType DotType => DotType.BeetleDot;
    public override int HitsToClear => 3;
    private DotColor color;
    public DotColor Color
    {
        get
        {
            if (HitCount > 0)
                return colors[Mathf.Clamp(HitCount, 0, HitsToClear - 1)];
            else
                return color;
        }
        set
        {
            color = value;
        }
    }

    public override DotsGameObjectData ReplacementDot
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

    private int directionX;
    private int directionY;
    public int DirectionX { get => directionX; set => directionX = value; }
    public int DirectionY { get => directionY; set => directionY = value; }
    private DotColor[] colors;
    public DotColor[] Colors { get => colors; set => colors = value; }
    

    public new BeetleDotVisualController VisualController => GetVisualController<BeetleDotVisualController>();


    public IEnumerator DoSwap(Dot dotToSwap)
    {
        if (WasHit)
        {
            WasHit = false;

            yield break;
        }

        yield return VisualController.DoSwap(dotToSwap);

    }

    public override void Hit(HitType hitType)
    {
        HitCount++;
        WasHit = true;
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
        
        yield return VisualController.PreviewHit(hitType);
        if(HitCount == 2)
        {
            StartCoroutine(PreviewClear());
        }
    }

    public IEnumerator PreviewClear()
    {
       yield return VisualController.PreviewClear();
    }

    public IEnumerator TrySwap()
    {
        if (WasHit)
        {
            WasHit = false;
            yield break;
        }
        

        yield return VisualController.TrySwap();
    }

    public override void Select()
    {
        base.Select();
        StartCoroutine(VisualController.PreviewHit(HitType.Connection));
    }

    

}
