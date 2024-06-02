using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class BeetleDot : ColorableDot
{
    public override DotType DotType => DotType.BeetleDot;

    public override int HitsToClear => 3;

   
    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new(base.HitRules)
            {
                {
                    HitType.BeetleDot, new HitByConnectionRule()
                },
            };
        }
    }

    public override IEnumerator Clear()
    {
        NotifyDotCleared();
        DotController.DoBombDot(this);
        yield return null;
    }


    public override IEnumerator Hit(HitType hitType)
    {
       
        if (hitType == HitType.BeetleDot || hitType == HitType.BombExplosion)
        {
            HitCount++;

        }
        
        yield return DoVisualHit(hitType);

        yield return base.Hit(hitType);
    }

    public override void InitDisplayController()
    {
        visualController = new BeetleDotVisualController();
        visualController.Init(this);
    }



}
