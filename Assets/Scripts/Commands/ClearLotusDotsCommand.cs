using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;
using System;
using System.Linq;

public class ClearLotusDotsCommand : Command
{
    public override CommandType CommandType => CommandType.ClearLotusDots;
    readonly HashSet<IColorable> visitedDots = new();
    private readonly List<Coroutine> coroutines = new();
    private int ongoingConnections = 0;

    private IEnumerator ConnectDots(IColorable startDot, Vector2Int direction, Board board, Action<Dot> onDotConnected)
    {
        // Queue for breadth-first search traversal
        Queue<IColorable> queue = new();

        // Enqueue the starting dot
        queue.Enqueue(startDot);

        // Perform breadth-first search
        while (queue.Count > 0)
        {
            IColorable currentDot = queue.Dequeue();
            visitedDots.Add(currentDot);

            if (currentDot is not IBoardElement b)
            {
                continue;
            }

            IColorable neighbor = board.GetDotAt<IColorable>(b.Column + direction.x, b.Row + direction.y);

            // Check if the neighbor is valid and has not been visited yet
            if (neighbor != null && !visitedDots.Contains(neighbor))
            {
                if (currentDot.Color != neighbor.Color)
                {
                    break;
                }
                else
                {
                    ConnectableDot dot = (ConnectableDot)neighbor;

                    if (dot is not LotusDot)
                    {
                        dot.Select();
                    }

                    ConnectionManager.ConnectDot(dot);
                    yield return LineManager.DrawLine(currentDot, neighbor);

                    onDotConnected?.Invoke((Dot)neighbor);

                    // Add the neighbor to the queue for further exploration
                    queue.Enqueue(neighbor);
                }

                visitedDots.Add(neighbor);
            }
        }

        ongoingConnections--;
    }
   
    private void ConnectInDirection(IColorable colorable, Board board, Action<Dot> onDotConnected = null, Action onComplete = null)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int direction in directions)
        {
            ongoingConnections++;
            CoroutineHandler.StartStaticCoroutine(ConnectDots(colorable, direction, board, dot =>
            {
                List<IColorable> neighbors = board.GetDotNeighbors<IColorable>(dot.Column, dot.Row, false);
                onDotConnected?.Invoke(dot);

                if (neighbors.Any((neighbor) => neighbor != null && neighbor.Color == colorable.Color))
                {
                    ConnectInDirection((IColorable)dot, board);
                }
            }));
        }
    }

    public override IEnumerator Execute(Board board)
    {
        ongoingConnections = 0;
        List<LotusDot> lotusDots = board.FindDotsOfType<LotusDot>();
       
        foreach (LotusDot lotusDot in lotusDots)
        {
            ConnectInDirection(lotusDot, board);
        }

        // Wait until all connections are done
        yield return new WaitUntil(() => ongoingConnections == 0);

        LineManager.RemoveAllLines();

        yield return base.Execute(board);
    }

}
