using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OneSidedBlock : HittableTile, IDirectional 
{
    
    public override TileType TileType => TileType.OneSidedBlock;
    private readonly DirectionalBase directional = new();

    private int directionX;
    private int directionY;
    public int DirectionX { get => directionX; set => directionX = value; }
    
    public int DirectionY { get => directionY; set => directionY = value; }
    public new OneSidedBlockVisualController VisualController => GetVisualController<OneSidedBlockVisualController>();
    public override IHitRule HitRule => new HitByNeighborRule();
    
    public override int HitsToClear => 1;


    public override void Init(int column, int row)
    {
        base.Init(column, row);
        directional.Init(this);

    }


    

    public override void Hit(HitType hitType)
    {
        HitCount++;
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
