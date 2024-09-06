using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Water : Tile
{
    public override TileType TileType => TileType.Water;


    public override void InitDisplayController()
    {
        visualController = new WaterVisualController();
        visualController.Init(this);
    }
}
