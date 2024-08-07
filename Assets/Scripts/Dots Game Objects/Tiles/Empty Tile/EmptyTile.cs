using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmptyTile : Tile
{
    public override TileType TileType => TileType.EmptyTile;


    public override void InitDisplayController()
    {
        visualController = new EmptyTileVisualController();
        visualController.Init(this);
    }


}
