using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit :  HittableTile, ISwitchable
{
    
    public override Dictionary<HitType, IHitRule> HitRules => new(){{HitType.Circuit, new HitBySamePositionRule()}};

   
    public override int HitsToClear => int.MaxValue;

    public override TileType TileType => TileType.Circuit;
    
    public bool IsActive {get; set;}

    public override void Hit(HitType hitType)
    {
        //switch to inactive if active
        if(IsActive){
            IsActive = false;
        }
        //switch to active if inactive
        else{
            IsActive = true;

        }
        
    }

    public override void InitDisplayController()
    {
        visualController = new CircuitVisualController();
        visualController.Init(this);
    }

    
}
