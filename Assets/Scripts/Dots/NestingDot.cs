using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;
public class NestingDot : Dot
{
    public override DotType DotType => DotType.NestingDot;
    private NestingDotVisualController VisualController
    {
        get
        {
            if (visualController is NestingDotVisualController nestingDotVisualController)
            {
                return nestingDotVisualController;
            }
            throw new InvalidCastException("Unable to cast base visualController to ClockDotVisualController");

        }
    }
    public override int HitsToClear => 3;

    public override Dictionary<HitType, IHitRule> HitRules =>
        new(){{HitType.NestingDot, new HitByNeighborsRule()}};


    public override IEnumerator Clear()
    {
        DotController.DoBombDot(this);
        yield break;
    }

    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        
        StartCoroutine(VisualController.Hit());

        return base.Hit(hitType);
    }
    public override void InitDisplayController()
    {
        visualController = new NestingDotVisualController();
        visualController.Init(this);
    }

}
