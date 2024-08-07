using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ClockDot : BlankDotBase, INumerable
{
    public override DotType DotType => DotType.ClockDot;
    public int TempNumber { get => numerable.TempNumber; set => numerable.TempNumber = value; }
    private readonly NumerableBase numerable = new();
    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }
    public int CurrentNumber { get => numerable.CurrentNumber;}

    public new ClockDotVisualController VisualController => GetVisualController<ClockDotVisualController>();
   
   
    public override DotsGameObjectData Replacement
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

    public override int HitsToClear =>int.MaxValue;


    public override void Init(int column, int row)
    { 
        base.Init(column, row);
        numerable.Init(this);

    }

    public void UpdateNumberVisuals(int number)
    {
       
        VisualController.UpdateNumbers(number);
        
    }

    public override void InitDisplayController()
    {
        visualController = new ClockDotVisualController();
        visualController.Init(this);
    }

    

    public void UpdateCurrentNumber(int number)
    {
        
        numerable.UpdateCurrentNumber(number);
    }
    
    public override void Disconnect()
    { 
        base.Disconnect();
        VisualController.UpdateNumbers(CurrentNumber);
    }


    public IEnumerator DoMove(List<Vector2Int> path, Action onMoved = null)
    {
        yield return VisualController.DoMove(path, onMoved);
        
        name = "(" + Column + ", " + Row + ")"; 
    }
    public override void Hit(HitType hitType)
    {
        numerable.Hit(hitType);
        
    }



    public override bool ShouldPreviewClear(Board board)
    {
        return TempNumber == 0;
    }


}
