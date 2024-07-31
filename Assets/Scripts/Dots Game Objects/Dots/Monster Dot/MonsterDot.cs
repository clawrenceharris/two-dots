using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterDot : ConnectableDot, IColorable, INumerable, IConnectable, IPreviewable, IDirectional
{

    private readonly NumerableBase numerable = new();

    public override DotType DotType => DotType.MonsterDot;
    private readonly DirectionalBase directional = new();
    public new MonsterDotVisualController VisualController => GetVisualController<MonsterDotVisualController>();
    public int TempNumber { get => numerable.TempNumber; set => numerable.TempNumber = value; }

    public int CurrentNumber => numerable.CurrentNumber;

    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }

    public override int HitsToClear => numerable.InitialNumber;

    public int DirectionX { get; set; }
    public int DirectionY { get; set; }
    public bool IsPreviewing { get; private set; }

    public List<IHitRule> PreviewHitRules => new() { new HitByConnectionRule()};

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
    public override IEnumerator Clear()
    {
        IsPreviewing = false;

        return base.Clear();
    }

    public override void Hit(HitType hitType)
    {
        numerable.Hit(hitType);
        HitCount = InitialNumber - TempNumber;

    }
    public override void Disconnect()
    {
        VisualController.UpdateNumbers(CurrentNumber);

    }

    public IEnumerator StartPreview(PreviewHitType hitType)
    {
        int connectionCount = ConnectionManager.ToHit.Count;

        TempNumber = Mathf.Clamp(CurrentNumber - connectionCount, 0, int.MaxValue);

        yield return VisualController.PreviewHit(hitType);
    }

    public IEnumerator PreviewClear()
    {
        yield break;
    }

    public void ChangeDirection(int directionX, int directionY)
    {
        directional.ChangeDirection(directionX, directionY);
    }

    public Vector3 GetRotation()
    {
        return directional.GetRotation();
    }

    public override void Deselect()
    {
        VisualController.UpdateNumbers(CurrentNumber);
    }

    public void StopPreview()
    {
       IsPreviewing = false;
    }
}
