using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

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
    public override int HitsToClear => 1;


    public override void InitDisplayController()
    {
        visualController = new AnchorDotVisualController();
        visualController.Init(this);
    }


    public override IEnumerator Hit(HitType hitType)
    {
        
        HitCount++;
        yield return base.Hit(hitType);
    }

}
