using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class MonsterDot : ConnectableDot, IColorable, INumerable, IConnectable, IPreviewable, IDirectional
{

    private readonly NumerableBase numerable = new();

    public override DotType DotType => DotType.MonsterDot;

    private readonly DirectionalBase directional = new();
    public new MonsterDotVisualController VisualController => GetVisualController<MonsterDotVisualController>();
    public int TempNumber{ get => numerable.TempNumber; set => numerable.TempNumber = value; }
   

    public int CurrentNumber => numerable.CurrentNumber;

    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }
    public DotColor Color { get; set; }

    

    public override int HitsToClear => numerable.InitialNumber;

    public int DirectionX { get; set; }
    public int DirectionY { get; set; }

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


    public IEnumerator DoMove(Action onComplete = null)
    {
        if (WasHit)
        {
            WasHit = false;
            yield break;
        }
        int targetCol = DirectionX + Column;
        int targetRow = DirectionY + Row;
        yield return VisualController.DoMove(targetCol, targetRow);
        onComplete?.Invoke();
    }

    public override void Hit(HitType hitType)
    {
        WasHit = true;
        numerable.Hit(hitType);
        HitCount = InitialNumber - TempNumber;

    }
    public override void Disconnect()
    {
        base.Disconnect();
        VisualController.UpdateNumbers(CurrentNumber);

    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        int connectionCount = ConnectionManager.ToHit.Count;
 
        TempNumber = Mathf.Clamp(CurrentNumber - connectionCount, 0, int.MaxValue);

        yield return VisualController.PreviewHit(hitType);
    }

    public IEnumerator PreviewClear()
    {
        yield break ;
    }

    public void ChangeDirection(int directionX, int directionY)
    {
        directional.ChangeDirection(directionX, directionY);
    }

    public Vector3 GetRotation()
    {
       return directional.GetRotation();
    }
}
