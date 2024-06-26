using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
using static Type;
using Object = UnityEngine.Object;
using Unity.VisualScripting;

public class DotsGameObjectController
{
    private static Board board;


    public DotsGameObjectController(Board board)
    {
        DotsGameObjectController.board = board;


    }

    public static Tween DropDot(Dot dot, int row, float duration = 0.6f)
    {
        dot.Row = row;
        dot.name = "(" + dot.Column + ", " + dot.Row + ")";
        return dot.transform.DOMoveY(row * Board.offset, duration).SetEase(Ease.OutBounce);
    }


    public static Tween Move<T>(T dotsObject, int col, int row, float duration = 0.5f)
        where T : DotsGameObject
    {

        return dotsObject.transform.DOMove(new Vector2(col, row) * Board.offset, duration).OnComplete(() =>
        {

            board.Put(dotsObject, col, row);
            dotsObject.Column = col;
            dotsObject.Row = row;
        });
        
       
       

       
    }

}
