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

    public static void PutDot(Dot dot)
    {
        board.PutDot(dot);
    }

    public static void RemoveDot(Dot dot)
    {
        board.RemoveDot(dot);
    }

    public static Tween DropDot(Dot dot, int row, float speed = 0.6f)
    {
        dot.Row = row;
        dot.name = "(" + dot.Column + ", " + dot.Row + ")";
        return dot.transform.DOMoveY(row * Board.offset, speed).SetEase(Ease.OutBounce);
    }


    public static void MoveDot(Dot dot, int col, int row, float speed = 0.5f)
    {
        board.MoveDot(dot, col, row);

        dot.transform.DOMove(new Vector2(col, row) * Board.offset, speed);
    }
    public static IEnumerator MoveDotThroughConnection(ConnectableDot start, ConnectableDot end, float speed = 0.8f)
    {

        LinkedList<ConnectableDot> dots = ConnectionManager.ConnectedDots;
        LinkedListNode<ConnectableDot> head = dots.First;
        if (!dots.Contains(start) || !dots.Contains(end))
        {
            throw new ArgumentException("Both dots must be within the connection.");
        }

        while (head.Value != end)
        {
            start.transform.DOMove(new Vector2(head.Value.Column, head.Value.Row) * Board.offset, speed);
            head = head.Next;
        }

        start.transform.DOMove(new Vector2(end.Column, end.Row) * Board.offset, speed);


        board.MoveDot(start, end.Column, end.Row);
        yield return null;

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
