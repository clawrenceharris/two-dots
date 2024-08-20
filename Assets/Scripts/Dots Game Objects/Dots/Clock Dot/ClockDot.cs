using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ClockDot : BlankDotBase, INumerable
{
    public override DotType DotType => DotType.ClockDot;
    private NumerableBase numerable;

    private NumerableBase Numerable {
        get {
            if (numerable == null) {
                numerable = GetComponent<NumerableBase>();
            }
            return numerable;
        }
    }
    public int TempNumber { get => Numerable.TempNumber; set => Numerable.TempNumber = value; }
    public int InitialNumber { get => Numerable.InitialNumber; set => Numerable.InitialNumber = value; }
    public int CurrentNumber { get => Numerable.CurrentNumber;}

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

    private void OnDestroy(){
        VisualController.Unsubscribe();
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
        
        Numerable.UpdateCurrentNumber(number);
    }
    
    


    public IEnumerator DoMove(List<Vector2Int> path, Action onMoved = null)
    {
        yield return VisualController.DoMove(path, onMoved);
        
        name = "(" + Column + ", " + Row + ")"; 
    }
    public override void Hit(HitType hitType)
    {
        Numerable.Hit(hitType);
        
    }



    public override bool ShouldPreviewClear(Board board)
    {
        return TempNumber == 0;
    }


}
