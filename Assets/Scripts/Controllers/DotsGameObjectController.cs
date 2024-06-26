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
<<<<<<< HEAD
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


        board.Put(start, end.Column, end.Row);
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

        board.Put(start, endCol, endRow);
        start.Column = endCol;
        start.Row = endRow;
    }
=======
    
>>>>>>> misc/fixes-and-refactoring

    

}
