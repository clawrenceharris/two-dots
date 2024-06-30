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

    public IEnumerator Hit(HitType hitType)
    {
        DotsObjectEvents.NotifyHit(this);

        HitType = hitType;
        HitCount++;
        if (hitType == HitType.BombExplosion)
        {
            yield return VisualController.DoBombHit();

        }
        yield return VisualController.DoHitAnimation(hitType);

    }


    public IEnumerator Clear(Action<IHittable> onComplete)
    {
        DotsObjectEvents.NotifyCleared(this, VisualController.Visuals.clearDuration);
        yield return VisualController.DoClearAnimation();
        onComplete?.Invoke(this);
    }


    public void UndoHit()
    {
        HitType = HitType.None;
    }
}
