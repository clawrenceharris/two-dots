using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit :  HittableTile
{
 
    public override Dictionary<HitType, IHitRule> HitRules => new(){{HitType.Circuit, new HitBySamePositionRule()}};

    public override int HitsToClear => int.MaxValue;

    public override TileType TileType => TileType.Circuit;

    public bool IsOn { get; private set; }

    public override void Hit(HitType hitType)
    {
        if(IsOn){
            IsOn = false;
        }
        else{
            IsOn = true;

        }
        
    }

    public override void InitDisplayController()
    {
        visualController = new CircuitVisualController();
        visualController.Init(this);
    }

    
}
