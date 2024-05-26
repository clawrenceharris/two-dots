using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class BlockTile : Tile, IHittable
{

    public override TileType TileType => TileType.BlockTile;
    public HitType HitType { get; private set; }
    public int HitCount { get; private set; }

    public int HitsToClear => 1;
    public Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
           return new() { {  HitType.BlockTile, new HitByNeighborsRule()} };
        }
    }

    public bool IsBomb => false;

    public override void Debug(Color color)
    {

        visualController.SpriteRenderer.color = color;

    }
    public void UndoHit()
    {
        HitType = HitType.None;
        HitCount--;
    }
    public IEnumerator Hit(HitType hitType)
    {
        HitType = hitType;
        HitCount--;        
        yield return null;

    }

    public void BombHit()
    {

        StartCoroutine(visualController.BombHit());
    }

   
}
