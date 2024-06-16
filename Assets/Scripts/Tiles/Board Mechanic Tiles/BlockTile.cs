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

    public bool IsBomb => false;

    public override void Debug(Color color)
    {

        visualController.SpriteRenderer.color = color;

    }
    
    public IEnumerator Hit(HitType hitType)
    {
        HitType = hitType;
        HitCount++;        
        yield return null;

    }

    public void BombHit()
    {

        StartCoroutine(visualController.BombHit());
    }

    public IEnumerator DoVisualHit(HitType hitType)
    {
        throw new System.NotImplementedException();
    }
}
