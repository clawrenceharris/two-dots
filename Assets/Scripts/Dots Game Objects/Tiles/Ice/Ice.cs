using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Ice : HittableTile
{
    public override TileType TileType => TileType.Ice;

    public new IceVisualController VisualController => GetVisualController<IceVisualController>();
    private readonly HittableBase hittable = new();

    public override int HitsToClear => 3;

    public override IHitRule HitRule => new HitBySamePositionRule();

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        hittable.Init(this);
    }
    public override void InitDisplayController()
    {
        visualController = new IceVisualController();
        visualController.Init(this);
    }

    

    public override void Hit(HitType hitType)
    {
        HitCount++;
    }

    
}
