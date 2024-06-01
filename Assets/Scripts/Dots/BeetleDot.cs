using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class BeetleDot : ConnectableDot, IColorable
{
    public override DotType DotType => DotType.BeetleDot;

    public override int HitsToClear => 3;

    private DotColor color;
    public DotColor Color { get => color; set => color = value; }

    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new(base.HitRules)
            {
                {
                    HitType.Connection, new HitByConnectionRule()
                },
            };
        }
    }

    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        return base.Hit(hitType);
    }

    public override void InitDisplayController()
    {
        visualController = new ColorDotVisualController();
        visualController.Init(this);
    }



}
