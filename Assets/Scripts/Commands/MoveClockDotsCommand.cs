using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using static Type;
public class MoveClockDotsCommand : Command
{
    private readonly LinkedList<ConnectableDot> connectedDots;

    public override CommandType CommandType => CommandType.MoveClockDots;

    public MoveClockDotsCommand(LinkedList<ConnectableDot> connectedDots)
    {
        this.connectedDots = connectedDots;
    }
    public override IEnumerator Execute(Board board)
    {

        LinkedListNode<ConnectableDot> currentNode = connectedDots.Last;

        Dictionary<ConnectableDot, Vector2Int> originalPositions = new();

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveClockDotsCommand));


        foreach (ConnectableDot dot in connectedDots)
        {
            originalPositions.Add(dot, new Vector2Int(dot.Column, dot.Row));
        }

        int count = 0;
        LinkedListNode<ConnectableDot> lastAvailableNode = connectedDots.Last;

        while (currentNode != null)
        {
            if (currentNode.Value is ClockDot clockDot)
            {
                DidExecute = true;

                count++;
                for (int i = 0; i < count - 1; i++)
                {
                    lastAvailableNode = lastAvailableNode.Previous;
                }

                // Build the path for the current ClockDot to move through
                List<Vector2Int> path = new();
                LinkedListNode<ConnectableDot> pathNode = currentNode;
                while (pathNode != lastAvailableNode.Next)
                {
                    if (originalPositions.TryGetValue(pathNode.Value, out Vector2Int originalPosition))
                    {
                        path.Add(originalPosition);
                    }
                    pathNode = pathNode.Next;
                }
                int col = clockDot.Column;
                int row = clockDot.Row;
<<<<<<< HEAD
                yield return CoroutineHandler.StartStaticCoroutine(DotsGameObjectController.MoveDotThroughConnection(clockDot, path, 0.2f));
=======

                yield return CoroutineHandler.StartStaticCoroutine(clockDot.DoMove(path, () =>
                {
                    int endCol = path[^1].x;
                    int endRow = path[^1].y;
                    board.Put(clockDot, endCol, endRow);
                    clockDot.Column = endCol;
                    clockDot.Row = endRow;
                }));
>>>>>>> misc/fixes-and-refactoring
                board.Remove(clockDot, col, row);

            }

            currentNode = currentNode.Previous;
        }
        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveClockDotsCommand));

            yield return new WaitForSeconds(0.7f);

        }

        yield return base.Execute(board);
    }
}
