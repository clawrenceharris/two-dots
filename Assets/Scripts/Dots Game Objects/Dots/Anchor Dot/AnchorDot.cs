using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorDot : Dot
{
    public override DotType DotType => DotType.AnchorDot;

    public override Dictionary<HitType, IHitRule> HitRules {
        get
        {
            return new()
            {
                { HitType.AnchorDot, new HitByBottomOfBoardRule() }
            };
        }
    }
    private new AnchorDotVisualController VisualController => GetVisualController<AnchorDotVisualController>();

    public override int HitsToClear => 1;


    public override void InitDisplayController()
    {
        visualController = new AnchorDotVisualController();
        visualController.Init(this);
    }


    public override void Hit(HitType hitType)
    {
        
        HitCount++;
    }

    
}
