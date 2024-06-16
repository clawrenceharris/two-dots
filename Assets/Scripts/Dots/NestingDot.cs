using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;
public class NestingDot : Dot
{
    public override DotType DotType => DotType.NestingDot;
    private NestingDotVisualController VisualController => GetVisualController<NestingDotVisualController>();
   
    public override int HitsToClear => 3;

    public override Dictionary<HitType, IHitRule> HitRules =>
        new(){{HitType.NestingDot, new HitByNeighborsRule()}};



    public override IEnumerator Hit(HitType hitType)
    {
        hitCount++;
        yield return DoVisualHit(hitType);
        yield return base.Hit(hitType);
    }
    public override void InitDisplayController()
    {
        visualController = new NestingDotVisualController();
        visualController.Init(this);
    }

}
