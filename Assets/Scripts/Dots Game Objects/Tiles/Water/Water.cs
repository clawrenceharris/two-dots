using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Water : Tile, IHittable
{
    public override TileType TileType => TileType.Water;

    private readonly HittableBase hittable = new();


    public HitType HitType { get => hittable.HitType; protected set => hittable.HitType = value; }

    public int HitCount { get => hittable.HitCount; set => hittable.HitCount = value; }

    public bool WasHit { get => hittable.WasHit; set => hittable.WasHit = value; }

    public int HitsToClear => int.MaxValue;

    public Dictionary<HitType, IHitRule> HitRules => new(){{HitType.Water, new HitBySamePositionRule()}};

    public IEnumerator Clear()
    {
        yield return hittable.Clear();
    }

    public IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        yield return Hit(hitType, onHitComplete);
    }

    public override void InitDisplayController()
    {
        visualController = new WaterVisualController();
        visualController.Init(this);
    }
}
