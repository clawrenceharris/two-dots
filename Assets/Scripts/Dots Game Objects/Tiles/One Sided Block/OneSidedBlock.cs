using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OneSidedBlock : Tile, IDirectional, IHittable
{
    
    public override TileType TileType => TileType.OneSidedBlock;
    private readonly DirectionalBase directional = new();

    private int directionX;
    private int directionY;
    public int DirectionX { get => directionX; set => directionX = value; }
    
    public int DirectionY { get => directionY; set => directionY = value; }
    public new OneSidedBlockVisualController VisualController => GetVisualController<OneSidedBlockVisualController>();
    public Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new() { { HitType.OneSidedBlock, new HitByNeighborRule()} };
        }
    }

    private readonly HittableBase hittable = new();


    public HitType HitType { get => hittable.HitType; }

    public int HitCount { get => hittable.HitCount; set => hittable.HitCount = value; }


    public int HitsToClear => 1;
    public bool WasHit { get => hittable.WasHit; set => hittable.WasHit = value; }

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        hittable.Init(this);
        directional.Init(this);

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
        visualController = new OneSidedBlockVisualController();
        visualController.Init(this);
    }



    
    public void ChangeDirection(int directionX, int directionY)
    {
        directional.ChangeDirection(directionX, directionY);
    }

    public Vector3 GetRotation()
    {
        return directional.GetRotation();
    }
}
