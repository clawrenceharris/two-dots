
using static Type;
using Color = UnityEngine.Color;
using System.Collections;
using System.Collections.Generic;
using System;

public class NormalDot : ColorableDot
{
    
    public override DotType DotType => DotType.NormalDot;

    public override int HitsToClear => 1;

    private new ColorableDotVisualController VisualController => GetVisualController<NormalDotVisualController>();
    
    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new()
            {

                {
                    HitType.Connection, new HitByConnectionRule()
                }

            };
        }
    }
    public override void Connect()
    {

        VisualController.AnimateSelectionEffect();

    }
    
   

   public override void InitDisplayController()
    {
        visualController = new NormalDotVisualController();
        visualController.Init(this);
    }


    

    
    public override void Select()
    {
        
        VisualController.AnimateSelectionEffect();

    }



    public override IEnumerator Hit(HitType hitType)
    {
        hitCount++;

        yield return base.Hit(hitType);
    }

    
}
