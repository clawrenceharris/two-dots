using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using static Type;
using Object = UnityEngine.Object;

public class DotController
{
    private static Board board;


    public DotController(Board b)
    {
        board = b;
        

    }

   

    public static Tween DropDot(Dot dot, int row, float speed = 0.6f)
    {
        dot.Row = row;
        dot.name = "(" + dot.Column + ", " + dot.Row + ")";
        return dot.transform.DOMoveY(row * Board.offset, speed).SetEase(Ease.OutBounce);
    }


    public static void MoveDot(Dot dot, int col, int row, float speed = 0.5f)
    {
        board.MoveDot(dot.Column, dot.Row, col, row);

        dot.transform.DOMove(new Vector2(col, row) * Board.offset, speed);
    }

    public static void ClearDot(Dot dot)
    {
        CoroutineHandler.StartStaticCoroutine(dot.Clear());

    }

    public static void DestroyDot(Dot dot)
    {
        Object.Destroy(dot.gameObject);
    }

    

}
