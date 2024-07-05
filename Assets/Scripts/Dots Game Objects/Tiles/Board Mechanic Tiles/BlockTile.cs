using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class Block : Tile, IHittable
{

    public override TileType TileType => TileType.Block;
    public int HitsToClear => 1;

    public Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
           return new() { {  HitType.BlockTile, new HitByNeighborsRule()} };
        }
    } 

    public new BlockVisualController VisualController => GetVisualController<BlockVisualController>();

    private readonly HittableBase hittable = new();


    public HitType HitType { get => hittable.HitType; }

    public int HitCount { get => hittable.HitCount; set => hittable.HitCount = value; }


    public bool WasHit { get => hittable.WasHit; set => hittable.WasHit = value; }

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        hittable.Init(this);

    }


    public IEnumerator Clear()
    {
        yield return hittable.Clear();

    }

    public virtual IEnumerator Hit(HitType hitType, Action onHitComplete = null)
    {
        HitCount++;

        yield return hittable.Hit(hitType, onHitComplete);
    }

    public override void InitDisplayController()
    {
        visualController = new BlockVisualController();
        visualController.Init(this);
    }
}
