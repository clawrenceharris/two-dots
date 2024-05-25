using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Object = UnityEngine.Object;

public class TileController
{

    private static Board board;


    public TileController(Board board)
    {
        TileController.board = board;
    }

    public static Tween DropDot(Tile tile, int row, float speed = 0.6f)
    {
        tile.Row = row;
        tile.name = "(" + tile.Column + ", " + tile.Row + ")";
        return tile.transform.DOMoveY(row * Board.offset, speed).SetEase(Ease.OutBounce);
    }
    public static void ClearTile(Tile tile)
    {

        CoroutineHandler.StartStaticCoroutine(tile.Clear());


    }

    public static void MoveTile(Tile tile, int col, int row, float speed = 0.5f)
    {
        board.MoveDot(tile.Column, tile.Row, col, row);

        tile.transform.DOMove(new Vector2(col, row) * Board.offset, speed);
    }


    public static void ClearTile(Tile tile, float clearTime)
    {
        CoroutineHandler.StartStaticCoroutine(tile.Clear());

    }

    public static void DestroyTile(Tile tile)
    {
        Object.Destroy(tile);
    }

}
