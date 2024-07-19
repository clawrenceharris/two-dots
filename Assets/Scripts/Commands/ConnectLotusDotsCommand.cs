using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;
using System;
using System.Linq;

/// <summary>
/// Command to connect Lotus dots and their neighbors of the same color on the board.
/// </summary>
public class ConnectLotusDotsCommand : Command
{
    public override CommandType CommandType => CommandType.ClearLotusDots;
    readonly HashSet<ConnectableDot> visitedDots = new();
    private int ongoingConnections = 0;

    /// <summary>
    /// Connects neighboring dots in a specific direction starting from the given dot.
    /// </summary>
    /// <param name="startingDot">The starting dot for the connection.</param>
    /// <param name="direction">The direction to connect to.</param>
    /// <param name="board">The game board.</param>
    /// <param name="onDotConnected">Action to perform when a dot is connected.</param>
    /// <returns>Enumerator for the coroutine.</returns>
    private IEnumerator ConnectNeighbors(ConnectableDot startingDot, Vector2Int direction, Board board, Action<ConnectableDot> onDotConnected)
    {
        // Queue for breadth-first search traversal
        Queue<ConnectableDot> queue = new();

        // Enqueue the starting dot
        queue.Enqueue(startingDot);

        // Perform breadth-first search
        while (queue.Count > 0)
        {
            ConnectableDot currentDot = queue.Dequeue();

            if (currentDot is not IBoardElement b)
            {
                continue;
            }

            ConnectableDot neighbor = board.GetDotAt<ConnectableDot>(b.Column + direction.x, b.Row + direction.y);

            // Checks that the neighbor is valid and has not been visited yet
            if (neighbor != null && !visitedDots.Contains(neighbor))
            {
                var rule = new LotusDotConnectionRule();
                if (rule.Validate(startingDot, neighbor, board))
                {
                    visitedDots.Add(neighbor);

                    break;
                }
                else
                {
                    visitedDots.Add(neighbor);


                    if (!neighbor.DotType.IsLotusDot())
                    {
                        neighbor.Select();
                    }

                    ConnectionManager.ConnectDot(neighbor);
                    yield return LineManager.DrawLine(currentDot, neighbor);

                    // Add the neighbor to the queue for further exploration
                    queue.Enqueue(neighbor);
                    onDotConnected?.Invoke(neighbor);

                   
                }

            }
        }

        ongoingConnections--;
    }

    /// <summary>
    /// Initiates connections from the given dot in all possible directions.
    /// </summary>
    /// <param name="startingDot">The starting dot for the connections.</param>
    /// <param name="board">The game board.</param>
    private void InitiateConnectionsInAllDirections(ConnectableDot startingDot, Board board)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int direction in directions)
        {
            ongoingConnections++;
            CoroutineHandler.StartStaticCoroutine(ConnectNeighbors(startingDot, direction, board, dot =>
            {
                List<IColorable> neighbors = board.GetDotNeighbors<IColorable>(dot.Column, dot.Row, false);

                if (neighbors.Any((neighbor) => neighbor != null && neighbor.Color == startingDot.Color))
                {
                    InitiateConnectionsInAllDirections(dot, board);
                }
            }));
        }
    }

    /// <summary>
    /// Executes the command to initiate the connection of Lotus dots on the board.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <returns>Enumerator for the coroutine.</returns>
    public override IEnumerator Execute(Board board)
    {
        ongoingConnections = 0;
        List<LotusDot> lotusDots = board.FindDotsOfType<LotusDot>();
       
        for(int i = 0; i < board.Width; i++)
        {
            for(int j = 0; j < board.Height; j++)
            {
                LotusDot lotusDot = board.GetDotAt<LotusDot>(i, j);

                if (lotusDot != null)
                {
                    InitiateConnectionsInAllDirections(lotusDot, board);

                }

            }
        }

        // Wait until all connections are done
        yield return new WaitUntil(() => ongoingConnections == 0);

        //remove the lines
        LineManager.RemoveAllLines();

        yield return base.Execute(board);
    }

}
