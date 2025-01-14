using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

/// <summary>
/// Command to connect Lotus dots and their neighbors of the same color on the board.
/// </summary>
public class ConnectLotusDotsCommand : Command
{
    public override CommandType CommandType => CommandType.ConnectLotusDots;
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
    private IEnumerator ConnectNeighbors(LotusDot lotusDot, ConnectableDot startingDot, Vector2Int direction, Board board, Action<ConnectableDot> onDotConnected)
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

            var neighbor = board.GetDotAt<ConnectableDot>(b.Column + direction.x, b.Row + direction.y);
            var rule = new LotusDotConnectionRule();
            if (neighbor != null && !visitedDots.Contains(neighbor))
            {
                //If the neighbor is not valid then exit the loop 
                if (!rule.Validate(lotusDot, neighbor, board))
                {
                    visitedDots.Add(neighbor);

                    break;
                }
               
                else
                {

                    if(neighbor.DotType.IsLotusDot()){
                        ConnectionManager.ConnectDot(new ConnectionArgs(dot: neighbor, playSound: false));
                    }
                    else{
                        ConnectionManager.SelectAndConnectDot(new ConnectionArgs(dot: neighbor, playSound: false));
                    }

                    
                    LineManager.DrawLine(currentDot, neighbor);
                    yield return new WaitForSeconds(0.3f);
                    // Add the neighbor to the queue for further exploration
                    queue.Enqueue(neighbor);
                    onDotConnected?.Invoke(neighbor);
                    
                    visitedDots.Add(neighbor);
                }
            }
            
        }
        ongoingConnections--;

    }

    /// <summary>
    /// Initiates connections from the given dot in all cardinal directions.
    /// </summary>
    /// <param name="startingDot">The starting dot for the connections.</param>
    /// <param name="board">The game board.</param>
    private void InitiateConnectionsInAllDirections(LotusDot lotusDot, ConnectableDot startingDot, Board board)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int direction in directions)
        {
            ongoingConnections++;
            CoroutineHandler.StartStaticCoroutine(ConnectNeighbors(lotusDot, startingDot, direction, board, dot =>
            {
                DidExecute = true;
                List<IColorable> neighbors = board.GetDotNeighbors<IColorable>(dot.Column, dot.Row, false);

                if (neighbors.Any((neighbor) => neighbor != null && neighbor.Color == startingDot.Color))
                {
                    InitiateConnectionsInAllDirections(lotusDot, dot, board);
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
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ConnectLotusDotsCommand));
        for(int i = 0; i < Board.Width; i++)
        {
            for(int j = 0; j < Board.Height; j++)
            {
                LotusDot lotusDot = board.GetDotAt<LotusDot>(i, j);

                if (lotusDot != null)
                {
                    InitiateConnectionsInAllDirections(lotusDot, lotusDot, board);

                }

            }
        }

        // Wait until all connections are done
        yield return new WaitUntil(() => ongoingConnections == 0);

        //remove the lines
        LineManager.RemoveAllLines();

        if(DidExecute){
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ConnectLotusDotsCommand));
        }
        yield return base.Execute(board);
    }

}
