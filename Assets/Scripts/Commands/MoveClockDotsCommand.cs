using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveClockDotsCommand : Command
{

    public override CommandType CommandType => CommandType.MoveClockDots;
    public override IEnumerator Execute(Board board)
    {
        
        IEnumerable<ConnectableDot> clockDots = ConnectionManager.ConnectedDots.Where(dot => dot.DotType.IsClockDot());

        if (clockDots.Count() == 0)
        {
            yield break;
        }
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveClockDotsCommand));


        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();

        Dictionary<ConnectableDot, Vector2Int> originalPositions = new();

        foreach (ConnectableDot dot in connectedDots)
        {
           
            originalPositions.TryAdd(dot, new Vector2Int(dot.Column, dot.Row));
        }
        DidExecute = true;

        int clockDotsMoved = 0;
        int count = 0;
        ConnectableDot lastAvailableDot;

        for (int i = connectedDots.Count-1; i >= 0; i--)
        {
            if (connectedDots[i] is ClockDot clockDot)
            {
                
                count++;
                int j = connectedDots.Count - count;

                lastAvailableDot = connectedDots[j];

                
                //while this current dot is not a basic dot
                while (!lastAvailableDot.DotType.IsBasicDot() && !lastAvailableDot.DotType.IsClockDot())
                {
                    //assign the last available dot to be the dot before this one
                    lastAvailableDot = connectedDots[Mathf.Clamp(j - 1, 0, connectedDots.Count - 1)];
                }

                List<Vector2Int> path = GetPath(i, connectedDots.IndexOf(lastAvailableDot));

                for(int k = 0; k <= clockDot.InitialNumber - clockDot.TempNumber; k++)
                {
                    yield return clockDot.Hit(HitType.Connection, null);

                }
                CoroutineHandler.StartStaticCoroutine(clockDot.DoMove(path), () =>
                {
                    if(path.Count > 0){
                        int startCol = path[0].x;
                        int startRow = path[0].y;
                        int endCol = path[^1].x;
                        int endRow = path[^1].y;
                        clockDot.Column = endCol;
                        clockDot.Row = endRow;

                    Dot dot = board.GetDotAt<Dot>(endCol, endRow);
                    if (dot != null && dot is not ClockDot)
                        board.DestroyDotsGameObject(dot);
                    board.Remove(clockDot, startCol, startRow);
                    board.Put(clockDot, endCol, endRow);
                    clockDotsMoved++;

                    if (clockDot.Column != startCol || clockDot.Row != startRow)
                    {
                        onCommandExecuting?.Invoke(this);
                    }                    }
                   
                });

                

            }
            

        }

        yield return new WaitForSeconds(ClockDotVisuals.moveDuration);

        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveClockDotsCommand));


        }

        yield return base.Execute(board);
    }

    private List<Vector2Int> GetPath(int startIndex, int endIndex)
    {
        List<ConnectableDot> connectedDots = ConnectionManager.Connection.ConnectedDots.ToList();

        Dictionary<ConnectableDot, Vector2Int> originalPositions = new();

        foreach (ConnectableDot dot in connectedDots)
        {

            originalPositions.TryAdd(dot, new Vector2Int(dot.Column, dot.Row));
        }
        // Build the path for the current Clock Dot to move through
        List<Vector2Int> path = new();
        for (int k = startIndex; k <= endIndex; k++)
        {
            if (originalPositions.TryGetValue(connectedDots[k], out Vector2Int originalPosition))
            {
                path.Add(originalPosition);
            }
        }
        return path;
    }
}
