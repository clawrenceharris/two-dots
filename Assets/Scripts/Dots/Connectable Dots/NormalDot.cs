
using static Type;
using Color = UnityEngine.Color;
using System.Collections;
using System.Collections.Generic;
using System;

public class NormalDot : ConnectableDot, IColorable
{
    private DotColor color;
    public DotColor Color { get => color; set => color = value; }
    public override DotType DotType => DotType.NormalDot;

    public override int HitsToClear => 1;

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


    public override void Connect()
    {

        VisualController.AnimateSelectionEffect();

    }
    
    public override void Disconnect()
    {
        HitCount = 0;
        base.Disconnect();

    }

   public override void InitDisplayController()
    {
        visualController = new ColorDotVisualController();
        visualController.Init(this);
    }


    

   
    public override void Select()
    {

        VisualController.AnimateSelectionEffect();

    }



    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;

        yield return base.Hit(hitType);
    }

    
}
