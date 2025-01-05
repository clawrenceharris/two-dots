using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BeetleDot : ConnectableDot, IDirectional, IPreviewable, IMultiColorable, ISwappable
{
    public override DotType DotType => DotType.BeetleDot;
    public override int HitsToClear => 3;
   
    private int Index => HitsToClear - HitCount -1;
    public override DotColor Color
    {
        get
        {
            return colors[Index];
            
        }
    }

    private readonly DirectionalBase directional = new();
    private readonly SwappableBase swappable = new();
    public int DirectionX { get => directional.DirectionX; set => directional.DirectionX = value; }
    public int DirectionY { get => directional.DirectionY; set => directional.DirectionY = value; }
    private DotColor[] colors;
    public DotColor[] Colors { get => colors; set => colors = value; }

    public new BeetleDotVisualController VisualController => GetVisualController<BeetleDotVisualController>();


    public override DotObject Replacement
    {
        get
        {
            return new()
            {
                type = LevelLoader.ToJsonDotType(DotType.Bomb),
                col = Column,
                row = Row
            };

        }
    }
    public override void Init(int column, int row)
    {
        directional.Init(this);
        swappable.Init(this);
        base.Init(column, row);
    }


    public override void Hit(HitType hitType)
    {
        HitCount++;
    }


    public override void InitDisplayController()
    {
        visualController = new BeetleDotVisualController();
        visualController.Init(this);
    }

    public IEnumerator TrySwap(Board board, Action<bool> onComplete)
    {
        DotsGameObject target = GetTarget(board);
        if(IsValidTarget(target, board)){
            yield return VisualController.DoSwap(target);
            onComplete?.Invoke(true);
        }
        else{
            Vector2Int newDirection = FindBestDirection(board, (target)=> IsValidTarget(target, board));
            yield return VisualController.TrySwap();
            ChangeDirection(newDirection.x, newDirection.y);
            onComplete?.Invoke(false);
        }
    }

    public override void Deselect()
    {
        //do nothing
    }


    public bool ShouldPreviewClear(Board board)
    {
        return HitCount == HitsToClear -1;
    }

    public bool ShouldPreviewHit(Board board)
    {
        return (HitRule.Validate(this, board) || 
        ConnectionManager.ConnectedDots.Contains(this)) && 
        DotTouchIO.IsInputActive;
    }

    
    public bool IsValidTarget(DotsGameObject target, Board board)
    {
        if(target == null || target is ISwappable){
            return false;
        }
        //if there is another swappable neighbor besides this one that has the same target as the given target
        if(target.FindDotNeighbors<ISwappable>(board).Count((swappable)=>swappable.GetTarget(board) == target) > 1){
            return false;
        }
        return true;
    }



    public Dot GetTarget(Board board)
    {
        return board.GetDotAt(Column + DirectionX, Row + DirectionY);
    }

    public Vector3 ToRotation(int directionX, int directionY)
    {
        return directional.ToRotation(directionX, directionY);
    }

        public Vector2Int FindBestDirection(Board board, Func<DotsGameObject, bool> isValidTarget)
    {
        return directional.FindBestDirection(board, isValidTarget);
    }

    public void ChangeDirection(int dirX, int dirY)
    {
        directional.ChangeDirection(dirX, dirY);
        StartCoroutine(VisualController.Rotate(ToRotation(DirectionX, DirectionY)));
    }

}
