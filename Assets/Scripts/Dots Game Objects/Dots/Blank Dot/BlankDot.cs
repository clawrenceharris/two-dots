
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlankDot : BlankDotBase
{
    public override DotType DotType => DotType.BlankDot;

    public override int HitsToClear => 1;



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
