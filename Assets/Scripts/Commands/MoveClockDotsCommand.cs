using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using static Type;
public class MoveClockDotsCommand : Command
{
    public override CommandType CommandType => CommandType.MoveClockDots;
    public static new bool DidExecute {get; private set;}
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveClockDotsCommand));
        LinkedList<ConnectableDot> connectedDots = new(ConnectionManager.ConnectedDots);
        LinkedListNode<ConnectableDot> currentNode = connectedDots.Last;

        Dictionary<ConnectableDot, Vector2Int> originalPositions = new();
        foreach (ConnectableDot dot in connectedDots)
        {
            originalPositions.Add(dot, new Vector2Int(dot.Column, dot.Row));
        }

        int count = 0;
        LinkedListNode<ConnectableDot> lastAvailableNode = connectedDots.Last;

        while (currentNode != null)
        {
            if (currentNode.Value is ClockDot)
            {
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

                CoroutineHandler.StartStaticCoroutine(DotController.MoveDotThroughConnection(currentNode.Value, path, 0.2f));
            }

            currentNode = currentNode.Previous;
        }

        //for (int i = connectedDots.Count - 1; i >= 0; i--)
        //{

        //    if (connectedDots[i] is ClockDot clockDot)
        //    {
        //        int count = 0;

        //        ConnectableDot lastEmptyDot = clockDot;
        //        for (int j = i; j < connectedDots.Count; j++)
        //        {

        //            if (connectedDots[j] is ClockDot)
        //            {
        //                count++;
        //                continue;
        //            }
        //            else
        //            {
        //                lastEmptyDot = connectedDots[j - count -1];
        //                DidExecute = true;
        //            }



        //        }



        //    }

        //}

        yield return new WaitForSeconds(1f);

        yield return base.Execute(board);
    }
}
