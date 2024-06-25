using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class OneSidedBlock : Tile, IDirectional, IHittable
{
    public override TileType TileType => TileType.OneSidedBlock;
    private int directionX;
    private int directionY;
    public HitType HitType { get; private set; }
    public int DirectionX { get => directionX; set => directionX = value; }

    public int DirectionY { get => directionY; set => directionY = value; }

    public Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new() { { HitType.OneSidedBlock, new HitByNeighborRule()} };
        }
    }


    private int hitCount;
    public int HitCount { get => hitCount; set => hitCount = value; }

    public int HitsToClear => 1;

    public override void InitDisplayController()
    {
        visualController = new OneSidedBlockVisualController();
        visualController.Init(this);
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

    public void UndoHit()
    {
        HitType = HitType.None;
    }
}
