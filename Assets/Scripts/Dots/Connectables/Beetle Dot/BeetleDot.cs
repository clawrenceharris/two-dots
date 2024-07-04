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

    private readonly DirectionalBase directional = new();

    
    public int DirectionX { get => directional.DirectionX; set => directional.DirectionX = value; }
    public int DirectionY { get => directional.DirectionY; set => directional.DirectionY = value; }
    private DotColor[] colors;
    public DotColor[] Colors { get => colors; set => colors = value; }
    

    public new BeetleDotVisualController VisualController => GetVisualController<BeetleDotVisualController>();


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

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        directional.Init(this);

    }

    public IEnumerator DoSwap(Dot dotToSwap, Action onComplete = null)
    {
        
        yield return VisualController.DoSwap(dotToSwap);
        onComplete?.Invoke();

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

    public void ChangeDirection(int directionX, int directionY)
    {
        directional.ChangeDirection(directionX, directionY);
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

    public IEnumerator TrySwap(Action onComplete = null)
    {
       

        yield return VisualController.TrySwap();
        onComplete?.Invoke();
    }

   
    void IDirectional.ChangeDirection(int directionX, int directionY)
    {
       directional.ChangeDirection(directionX, directionY);
    }

    public Vector3 GetRotation()
    {
        return directional.GetRotation();
    }

    public override void Deselect()
    {
        //do nothing
    }

    public override void Disconnect()
    {
        //do nothing
    }
}
