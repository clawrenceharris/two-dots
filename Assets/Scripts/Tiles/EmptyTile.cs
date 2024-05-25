using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class EmptyTile : Tile
{
    public override TileType TileType => TileType.EmptyTile;

    
    public override void InitDisplayController()
    {
        visualController = new EmptyTileVisualController();
        visualController.Init(this);
    }

    public override void Debug(Color color)
    {

        visualController.SpriteRenderer.color = color;

    }

}
