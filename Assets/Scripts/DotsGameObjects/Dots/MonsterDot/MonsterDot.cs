using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterDot : ConnectableDot, IColorable, INumerable, IConnectable, IDirectional, IPreviewable
{


    public override DotType DotType => DotType.MonsterDot;
    public new MonsterDotVisualController VisualController => GetVisualController<MonsterDotVisualController>();

    private NumerableBase numerable;

    private NumerableBase Numerable {
        get {
            if (numerable == null) {
                numerable = GetComponent<NumerableBase>();
            }
            return numerable;
        }
    }

    public override int HitsToClear => Numerable.InitialNumber;
    public int TempNumber { get => Numerable.TempNumber; set => Numerable.TempNumber = value; }

    public int CurrentNumber => Numerable.CurrentNumber;

    public int InitialNumber { get => Numerable.InitialNumber; set => Numerable.InitialNumber = value; }


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
   

   
   
    public void ChangeDirection(int directionX, int directionY)
    {
        directional.ChangeDirection(directionX, directionY);
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
        
    }

    public IEnumerator UpdateCurrentNumber(int number)
    {
        yield return Numerable.UpdateCurrentNumber(number);
    }

    public Vector2Int FindBestDirection(Board board, Func<DotsGameObject, bool> isValidTarget)
    {
        throw new NotImplementedException();
    }

    public Vector3 ToRotation(int dirX, int dirY)
    {
        return directional.ToRotation(dirX, dirY);
    }
}
