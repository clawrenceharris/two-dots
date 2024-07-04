using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using static Type;
public class MoveClockDotsCommand : Command
{

    public override CommandType CommandType => CommandType.MoveClockDots;
    public override IEnumerator Execute(Board board)
    {
        

        if (!ConnectionManager.Connection.Any<ClockDot>())
        {
            yield break;
        }
        onCommandExecuting?.Invoke(this);

        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();

        Dictionary<ConnectableDot, Vector2Int> originalPositions = new();

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveClockDotsCommand));
        foreach (ConnectableDot dot in connectedDots)
        {
           
            originalPositions.TryAdd(dot, new Vector2Int(dot.Column, dot.Row));
        }
        DidExecute = true;

        int count = 0;
        ConnectableDot lastAvailableDot = connectedDots[^1];

        for (int i = connectedDots.Count-1; i >= 0; i--)
        {
            if (connectedDots[i] is ClockDot clockDot)
            {

                count++;
                
                lastAvailableDot = connectedDots[^count];

                

                // Build the path for the current Clock Dot to move through
                List<Vector2Int> path = new();
                for(int k = i; k <= connectedDots.IndexOf(lastAvailableDot); k++)
                {
                    if (originalPositions.TryGetValue(connectedDots[k], out Vector2Int originalPosition))
                    {
                        path.Add(originalPosition);
                    }
                }
               
               

                CoroutineHandler.StartStaticCoroutine(clockDot.DoMove(path, () =>
                {
                    int startCol = path[0].x;
                    int startRow = path[0].y;
                    int endCol = path[^1].x;
                    int endRow = path[^1].y;
                    clockDot.Column = endCol;
                    clockDot.Row = endRow;
                    
                    Dot dot = board.Get<Dot>(endCol, endRow);
                    if(dot != null && dot is not ClockDot)
                        board.DestroyDotsGameObject(dot);
                    board.Put(clockDot, endCol, endRow);
                    board.Remove(clockDot, startCol, startRow);


                }));

            }
            

        }

        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveClockDotsCommand));
            yield return new WaitForSeconds(0.7f);


        }

        yield return base.Execute(board);
    }
}
