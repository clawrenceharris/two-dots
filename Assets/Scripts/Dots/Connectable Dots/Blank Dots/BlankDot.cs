
using System.Collections;
using System.Collections.Generic;
using static Type;
using UnityEngine;

public class BlankDot : BlankDotBase
{
    public override DotType DotType => DotType.BlankDot;

    public override int HitsToClear => 1;

    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new(base.HitRules)
            {

                {
                    HitType.Connection, new HitByConnectionRule()
                }

            };
        }
    }



    public override void Disconnect()
    {
        base.Disconnect();
    }


    public override void InitDisplayController()
    {
        visualController = new BlankDotVisualController();
        visualController.Init(this);
    }
    
    public override IEnumerator Hit(HitType hitType)
    {
        hitCount++;
        yield return base.Hit(hitType);
    }


}
