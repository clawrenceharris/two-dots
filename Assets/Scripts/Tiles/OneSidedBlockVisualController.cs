using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class OneSidedBlockVisualController : TileVisualController
{
    private new OneSidedBlock Tile; 
    public override void Init(Tile tile)
    {
        Tile = (OneSidedBlock)tile;

        base.Init(Tile);
        SetUp();
    }

    private void SetUp()
    {
        Rotate();
    }

    private void Rotate()
    {
        Quaternion rotation = Quaternion.identity;

        if(Tile.DirectionX < 0)
        {
            SpriteRenderer.flipY = true;

            rotation = Quaternion.Euler(0, 0, 180);
        }
        else if(Tile.DirectionY < 0)
        {
            rotation = Quaternion.Euler(0, 0, -90);
        }
        else if(Tile.DirectionY >0)
        {
            rotation = Quaternion.Euler(0, 0, 90);

        }

        Tile.transform.rotation = rotation;
    }
}
