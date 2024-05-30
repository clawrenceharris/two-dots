using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class NestingDot : Dot
{
    public override DotType DotType => DotType.NestingDot;

    public override int HitsToClear => 3;

    public override Dictionary<HitType, IHitRule> HitRules => throw new System.NotImplementedException();

    public override void InitDisplayController()
    {
        visualController = new DotVisualController();
        visualController.Init(this);
    }
}
