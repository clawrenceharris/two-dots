using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : HittableTile
{

    public override TileType TileType => TileType.Block;
    public override int HitsToClear => 1;

    public override IHitRule HitRule => new HitByNeighborsRule();
        
     

    public new BlockVisualController VisualController => GetVisualController<BlockVisualController>();




    public override void Hit(HitType hitType)
    {
        HitCount++;

    }

    public override void InitDisplayController()
    {
        visualController = new BlockVisualController();
        visualController.Init(this);
    }
}
