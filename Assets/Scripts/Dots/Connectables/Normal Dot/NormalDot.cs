
using static Type;
using Color = UnityEngine.Color;
using System.Collections;
using System.Collections.Generic;
using System;

public class NormalDot : ConnectableDot, IColorable, IConnectable
{

    public override DotType DotType => DotType.NormalDot;

    public override int HitsToClear => 1;

    private new NormalDotVisualController VisualController => GetVisualController<NormalDotVisualController>();
    
    
    public override void Init(int column, int row)
    {
        base.Init(column, row);
    }

    public DotColor Color { get; set; }


    public override void InitDisplayController()
    {
        visualController = new NormalDotVisualController();
        visualController.Init(this);

    }


    public override void Deselect()
    {
        //do nothing
    }


    public override void Hit(HitType hitType)
    {
        HitCount++;
    }

    public override void Disconnect()
    {
        //do nothing;
    }
}
