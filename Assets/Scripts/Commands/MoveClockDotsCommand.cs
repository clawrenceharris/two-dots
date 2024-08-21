using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveClockDotsCommand : Command
{
    public override CommandType CommandType => CommandType.MoveClockDots;
    private readonly List<ClockDot> visitedDots = new();
    private ConnectableDot FindLastAvailableDot(int clockDotCount){
        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();


        ConnectableDot lastAvailableDot = connectedDots[^clockDotCount];
       
        int lastIndex = connectedDots.IndexOf(lastAvailableDot);
        //while this current dot is not a basic dot
        while (!lastAvailableDot.DotType.IsBasicDot() && !lastAvailableDot.DotType.IsClockDot())
        {   
            //assign the last available dot to be the dot before this one
            lastAvailableDot = connectedDots[Mathf.Clamp(lastIndex - 1, 0, connectedDots.Count - 1)];
        }
            
        return lastAvailableDot;
    }
    
    private IEnumerator MoveClockDots(Board board){
        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();
        List<ClockDot> clockDotsMoved = new();
        
        int ongoingCoroutines = 0;        
        int clockDotCount = 0;

        if(!connectedDots[^1].DotType.IsBasicDot() && !ConnectionManager.IsSquare){
            clockDotCount++;
        }
        for (int i = connectedDots.Count -2; i >= 0; i--)
        { 
            if (connectedDots[i] is not ClockDot clockDot){
                continue;
            }

            clockDotCount++;
            
            ConnectableDot startDot = connectedDots[i + 1];
            ConnectableDot endDot = FindLastAvailableDot(clockDotCount);
            
            List<Vector2Int> path = new()
            {
                new Vector2Int(connectedDots[i].Column, connectedDots[i].Row)
            };

            path = path.Concat(GeneratePath(startDot, endDot)).ToList();
            
            ongoingCoroutines++;      
            CoroutineHandler.StartStaticCoroutine(clockDot.DoMove(path), () =>
            {
               
                clockDotsMoved.Add(clockDot);
                UpdateClockDotPosition(clockDot, path, board);
                ongoingCoroutines--;

                
            });

            

        }
        
        yield return new WaitUntil(()=> ongoingCoroutines == 0);
        
        //put each clock dot on the board at its current position
        foreach(ClockDot dot in clockDotsMoved){
            board.Put(dot, dot.Column, dot.Row);   
        }
    
    }

    private void UpdateClockDotPosition(ClockDot clockDot, List<Vector2Int> path, Board board){
         if(path.Count > 0){
            int startCol = path[0].x;
            int startRow = path[0].y;
            int endCol = path[^1].x;
            int endRow = path[^1].y;
            clockDot.Column = endCol;
            clockDot.Row = endRow;
            Dot dot = board.GetDotAt<Dot>(endCol, endRow);
            if (dot && dot is not ClockDot)
                board.DestroyDotsGameObject(dot);
            board.Remove(clockDot, startCol, startRow);
            if (clockDot.Column != startCol || clockDot.Row != startRow)
            {
                onCommandExecuting?.Invoke(this);
            }  
        }
    }

    public override IEnumerator Execute(Board board)
    {
        
        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();

        if (connectedDots.Count(dot => dot.DotType.IsClockDot()) == 0)
        {
            yield break;
        }
       
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveClockDotsCommand));
       
        DidExecute = true;
        
        yield return MoveClockDots(board);
        yield return new WaitForSeconds(ClockDotVisuals.moveDuration);

        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveClockDotsCommand));


        }

        yield return base.Execute(board);
    }

    private List<Vector2Int> GeneratePath(ConnectableDot startDot, ConnectableDot endDot)
    {
        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();
        List<Vector2Int> path = new();
        Dictionary<ConnectableDot, Vector2Int> originalPositions = new();
        for (int i = 0; i < connectedDots.Count; i++)
        {

            originalPositions.TryAdd(connectedDots[i], new Vector2Int(connectedDots[i].Column, connectedDots[i].Row));
        }       
        int startIndex = connectedDots.LastIndexOf(startDot);
        int endIndex = connectedDots.LastIndexOf(endDot);
        
        for (int j = startIndex; j <= endIndex; j++)
        {
            if (originalPositions.TryGetValue(connectedDots[j], out Vector2Int pos))
            {
                path.Add(pos);
            }

        }

        return path;
    }

}
