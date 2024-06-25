
using static Type;
using Color = UnityEngine.Color;
using System.Collections;
using System.Collections.Generic;
using System;

public class NormalDot : ConnectableDot, IColorable, IConnectable
{
    //private readonly ColorableBase colorable = new();

    public override DotType DotType => DotType.NormalDot;

    public override int HitsToClear => 1;

<<<<<<< Updated upstream:Assets/Scripts/Dots/Connectable Dots/NormalDot.cs
    private ColorDotVisualController VisualController
    {
        get
        {
            if (visualController is ColorDotVisualController colorDotVisualController)
            {
                return colorDotVisualController;
            }
            throw new InvalidCastException("Unable to cast base visualController to ClockDotVisualController");

        }
    }

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
=======
    private new ColorableDotVisualController VisualController => GetVisualController<NormalDotVisualController>();
    
>>>>>>> Stashed changes:Assets/Scripts/Dots/Connectables/NormalDot.cs
    
    public override void Init(int column, int row)
    {
        base.Init(column, row);
        //colorable.Init();
    }

    public DotColor Color { get; set; }

    public override void InitDisplayController()
    {
        visualController = new ColorDotVisualController();
        visualController.Init(this);

    }





    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;

        yield return base.Hit(hitType);
    }

}
