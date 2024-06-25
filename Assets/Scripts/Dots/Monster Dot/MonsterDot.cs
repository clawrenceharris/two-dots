using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class MonsterDot : ConnectableDot, IColorable, INumerable, IConnectable, IPreviewable
{

    private int tempNumber;
    private readonly NumerableBase numerable = new();

    public override DotType DotType => DotType.MonsterDot;


    public new MonsterDotVisualController VisualController => GetVisualController<MonsterDotVisualController>();

    public int TempNumber => tempNumber;

    public int CurrentNumber => numerable.CurrentNumber;

    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }
    public DotColor Color { get; set; }

    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new()
            {
                {
                    HitType.MonsterDot, new HitByConnectionRule()
                },
                {
                    HitType.Square, new HitBySquareRule()
                }
            };
        }
    }

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
        StartCoroutine(VisualController.Hit(hitType));

        
        //this happens when the connection has concluded  
        if (hitType == HitType.MonsterDot)
        {
            numerable.UpdateCurrentNumber(tempNumber);
            HitCount = InitialNumber - tempNumber;
            tempNumber = 0;

        }
        else if (hitType == HitType.BombExplosion)
        {
            //set current number to be one less than the current number
            numerable.UpdateCurrentNumber(Mathf.Clamp(CurrentNumber - 1, 0, int.MaxValue));
            HitCount++;

        }

        yield return VisualController.Hit(hitType);

    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        int connectionCount = ConnectionManager.ToHit.Count;

        tempNumber = Mathf.Clamp(CurrentNumber - connectionCount, 0, int.MaxValue);

        yield return  VisualController.PreviewHit(hitType);
    }

    public IEnumerator PreviewClear()
    {
        yield break ;
    }
}
