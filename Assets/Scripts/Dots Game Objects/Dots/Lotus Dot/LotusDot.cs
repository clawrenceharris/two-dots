using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LotusDot : ConnectableDot
{
    public override DotType DotType => DotType.LotusDot;
    public override int HitsToClear => 1;


    public override void Deselect()
    {
       //do nothing
    }

    public override void Connect(ConnectableDot dot)
    {
        //do nothing
    }

    public override void Hit(HitType hitType)
    {
        HitCount++;
    }

    public override void InitDisplayController()
    {
        visualController = new LotusDotVisualController();
        visualController.Init(this);
    }

    
}
