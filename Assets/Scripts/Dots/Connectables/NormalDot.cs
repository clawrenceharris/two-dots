
using static Type;
using Color = UnityEngine.Color;
using System.Collections;
using System.Collections.Generic;
using System;

public class NormalDot : ConnectableDot, IColorable, IConnectable
{

    public override DotType DotType => DotType.NormalDot;

    public override int HitsToClear => 1;

    private new ColorableDotVisualController VisualController => GetVisualController<NormalDotVisualController>();
    
    
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





    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;

        yield return base.Hit(hitType);
    }

}
