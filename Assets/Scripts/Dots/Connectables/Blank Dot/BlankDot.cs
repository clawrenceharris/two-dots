
using System.Collections;
using System.Collections.Generic;
using static Type;
using UnityEngine;
using System;

public class BlankDot : BlankDotBase
{
    public override DotType DotType => DotType.BlankDot;

    public override int HitsToClear => 1;
    private new BlankDotVisualController VisualController => GetVisualController<BlankDotVisualController>();



    public override void InitDisplayController()
    {
        visualController = new BlankDotVisualController();
        visualController.Init(this);
    }

    public override void Hit(HitType hitType)
    {
        HitCount++;
    }

   
}
