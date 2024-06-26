using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class BlockTile : Tile, IHittable
{

    public override TileType TileType => TileType.BlockTile;
    public HitType HitType { get; private set; }
    private int hitCount;
    public int HitCount { get => hitCount; set => hitCount = value; }
    public int HitsToClear => 1;
    public bool WasHit { get; protected set; }

    public Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
           return new() { {  HitType.BlockTile, new HitByNeighborsRule()} };
        }
    } 

    public new BlockTileVisualController VisualController => GetVisualController<BlockTileVisualController>();


    public bool DidExecute { get; private set; }

    public override void InitDisplayController()
    {
        visualController = new BlockTileVisualController();
        visualController.Init(this);
    }

    public IEnumerator Hit(HitType hitType, Action onHitChanged = null)
    {
        DotsObjectEvents.NotifyHit(this);

        HitType = hitType;
        HitCount++;
        if (hitType == HitType.BombExplosion)
        {
            yield return VisualController.DoBombHit();

        }
        onHitChanged?.Invoke();

        yield return VisualController.DoHitAnimation(hitType);

    }


    public IEnumerator Clear()
    {
        DotsObjectEvents.NotifyCleared(this);
        yield return VisualController.DoClearAnimation();
    }


    public void UndoHit()
    {
        HitType = HitType.None;
    }
}
