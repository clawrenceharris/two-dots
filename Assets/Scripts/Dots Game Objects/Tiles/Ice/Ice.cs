using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;
public class Ice : Tile, IHittable
{
    public override TileType TileType => TileType.Ice;

    public new IceVisualController VisualController => GetVisualController<IceVisualController>();
    private readonly HittableBase hittable = new();

    public HitType HitType { get => hittable.HitType;}
    public bool WasHit { get => hittable.WasHit; set => hittable.WasHit = value; }
    public int HitCount { get => hittable.HitCount; set => hittable.HitCount = value; }

    public int HitsToClear => 3;

    public Dictionary<HitType, IHitRule> HitRules => new() { { HitType.Connection, new HitBySamePositionRule() } };

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

    public IEnumerator Clear()
    {
       yield return hittable.Clear();
    }

    public IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        HitCount++;
        yield return hittable.Hit(hitType, onHitComplete);
    }

    
}
