using System.Collections;
using UnityEngine;
using static Type;
using System;
using System.Collections.Generic;

public class ClockDot : BlankDotBase, INumerable, IPreviewable
{
    public override DotType DotType => DotType.ClockDot;
    public int TempNumber { get => numerable.TempNumber; set => numerable.TempNumber = value; }
    private readonly NumerableBase numerable = new();
    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }
    public int CurrentNumber { get => numerable.CurrentNumber;}
    public new ClockDotVisualController VisualController => GetVisualController<ClockDotVisualController>();

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

    public override int HitsToClear => numerable.InitialNumber;


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
        VisualController.Disconnect();
    }

    public IEnumerator DoMove(List<Vector2Int> path, Action onComplete)
    {
        yield return VisualController.DoMove(path, onComplete);
        
        name = "(" + Column + ", " + Row + ")"; 
    }

    public override IEnumerator Hit(HitType hitType)
    {

        numerable.Hit(hitType);
        HitCount = InitialNumber - TempNumber;
        yield return base.Hit(hitType);

    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        int connectionCount = ConnectionManager.ToHit.Count;

        TempNumber = Mathf.Clamp(CurrentNumber - connectionCount, 0, int.MaxValue);

        if(InitialNumber - TempNumber == HitsToClear)
        {
            StartCoroutine(PreviewClear());
        }
        yield return VisualController.PreviewHit(hitType);
     
    }

    public IEnumerator PreviewClear()
    {
        yield return VisualController.PreviewClear();
    }

    



}
