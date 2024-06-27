using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class MonsterDot : ConnectableDot, IColorable, INumerable, IConnectable, IPreviewable
{

    private readonly NumerableBase numerable = new();

    public override DotType DotType => DotType.MonsterDot;


    public new MonsterDotVisualController VisualController => GetVisualController<MonsterDotVisualController>();
    public int TempNumber{ get => numerable.TempNumber; set => numerable.TempNumber = value; }

    public int CurrentNumber => numerable.CurrentNumber;

    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }
    public DotColor Color { get; set; }

    

    public override int HitsToClear => numerable.InitialNumber;


    public override void Init(int column, int row)
    {
        base.Init(column, row);
        numerable.Init(this);
    }


    public override void InitDisplayController()
    {
        visualController = new MonsterDotVisualController();
        visualController.Init(this);
    }

    public override IEnumerator Hit(HitType hitType)
    {

        numerable.Hit(hitType);
        HitCount = InitialNumber - TempNumber;

        yield return VisualController.Hit(hitType);

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

    
}
