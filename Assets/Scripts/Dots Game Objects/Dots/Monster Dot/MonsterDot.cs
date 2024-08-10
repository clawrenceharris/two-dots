using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterDot : ConnectableDot, IColorable, INumerable, IConnectable, IDirectional, IPreviewable
{


    public override DotType DotType => DotType.MonsterDot;
    public new MonsterDotVisualController VisualController => GetVisualController<MonsterDotVisualController>();
    public override int HitsToClear => numerable.InitialNumber;

    private readonly NumerableBase numerable = new();

    public int TempNumber { get => numerable.TempNumber; set => numerable.TempNumber = value; }

    public int CurrentNumber => numerable.CurrentNumber;

    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }


    private readonly DirectionalBase directional = new();

    public int DirectionX { get => directional.DirectionX; set => directional.DirectionX = value; }
    public int DirectionY { get => directional.DirectionY; set => directional.DirectionY = value; }




    public override void Init(int column, int row)
    {
        base.Init(column, row);
        numerable.Init(this);
        directional.Init(this);
    }


    public override void InitDisplayController()
    {
        visualController = new MonsterDotVisualController();
        visualController.Init(this);
    }


    public IEnumerator DoMove()
    {

        int targetCol = DirectionX + Column;
        int targetRow = DirectionY + Row;
        yield return VisualController.DoMove(targetCol, targetRow);
    }
    

    public override void Hit(HitType hitType)
    {
        
        numerable.Hit(hitType);
        HitCount = InitialNumber - TempNumber;

    }
    public override void Disconnect()
    {
        numerable.Disconnect();

    }

   
   
    public void ChangeDirection(int directionX, int directionY)
    {
        directional.ChangeDirection(directionX, directionY);
    }

    public Vector3 GetRotation()
    {
        return directional.GetRotation();
    }

    public bool ShouldPreviewClear(Board board)
    {
       return false;
    }

    public bool ShouldPreviewHit(Board board)
    {
        return false;
    }

    public override void Deselect()
    {
        throw new NotImplementedException();
    }
}
