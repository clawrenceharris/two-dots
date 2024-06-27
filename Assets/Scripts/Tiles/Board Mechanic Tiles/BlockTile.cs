using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class BlockTile : Tile, IHittable, ICommand
{

    public override TileType TileType => TileType.BlockTile;
    public HitType HitType { get; private set; }
    private int hitCount;
    public int HitCount { get => hitCount; set => hitCount = value; }
    public int HitsToClear => 1;
    public Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
           return new() { {  HitType.BlockTile, new HitByNeighborsRule()} };
        }
    } 

    public new BlockTileVisualController VisualController => GetVisualController<BlockTileVisualController>();

    public CommandType CommandType => CommandType.BlockTile;

    public bool DidExecute { get; private set; }

    public override void InitDisplayController()
    {
        visualController = new BlockTileVisualController();
        visualController.Init(this);
    }

    public IEnumerator Hit(HitType hitType)
    {
        HitType = hitType;
        hitCount++;
        DotsObjectEvents.NotifyHit(this);
        yield return VisualController.Hit(hitType);


    }


    public IEnumerator Clear()
    {
        DotsObjectEvents.NotifyCleared(this);
        yield return VisualController.Clear();
    }

    public void UndoHit()
    {
        HitType = HitType.None;
    }

    public IEnumerator Execute(Board board)
    {
        throw new NotImplementedException();
    }
}
