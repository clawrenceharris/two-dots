using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
using static Type;
using Object = UnityEngine.Object;
using Unity.VisualScripting;

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
    public static IEnumerator MoveDotThroughConnection(ConnectableDot start, ConnectableDot end, float speed = 0.5f)
    {

        LinkedList<ConnectableDot> dots = ConnectionManager.ConnectedDots;
        
        if (!dots.Contains(start) || !dots.Contains(end))
        {
            throw new ArgumentException("Both dots must be within the connection.");
        }

        LinkedListNode<ConnectableDot> startNode = dots.Find(start);
        LinkedListNode<ConnectableDot> endNode = dots.Find(end);

        while (startNode != endNode)
        {
            startNode = startNode.Next;

            start.transform.DOMove(new Vector2(startNode.Value.Column, startNode.Value.Row) * Board.offset, speed);
            yield return new WaitForSeconds(speed - speed/2);

        }

        start.transform.DOMove(new Vector2(end.Column, end.Row) * Board.offset, speed);


        board.MoveDot(start, end.Column, end.Row);
        start.Column = end.Column;
        start.Row = end.Row;

    }
    public static IEnumerator MoveDotThroughConnection(ConnectableDot start, List<Vector2Int> path, float speed = 0.5f)
    {
        foreach (var pos in path)
        {
            start.transform.DOMove(new Vector2(pos.x, pos.y) * Board.offset, speed);
            yield return new WaitForSeconds(speed - speed / 2);
        }

        // Update the final position
        int endCol = path[^1].x;
        int endRow = path[^1].y;

        board.MoveDot(start, endCol, endRow);
        start.Column = endCol;
        start.Row = endRow;
    }

    public static void ClearDot(Dot dot)
    {
        CoroutineHandler.StartStaticCoroutine(dot.Clear());

    }

    public static void DestroyDot(Dot dot)
    {
        Object.Destroy(dot.gameObject);
    }

    public static void DoBombDot(Dot dot)
    {
        DestroyDot(dot);
        board.CreateBomb(dot.Column, dot.Row);
    }

    
}
