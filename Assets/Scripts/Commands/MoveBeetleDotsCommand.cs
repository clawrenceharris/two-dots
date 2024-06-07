using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class MoveBeetleDotsCommand : Command
{
    public override CommandType CommandType => CommandType.MoveBeetleDots;

    private bool CanMove(Dot dotToSwap, BeetleDot beetleDot, Board board)
    {


        if(dotToSwap == null || dotToSwap is BeetleDot)
        {
            return false;
        }
        List<Dot> dotToSwapNeighbors = board.GetDotNeighbors(dotToSwap.Column, dotToSwap.Row);

        //if there is another beetle dot beside the dot to swap and they are going towards the same dot then it cannot move 
        foreach (Dot neighbor in dotToSwapNeighbors)
        {
            
            if (neighbor is BeetleDot beetleDotNeighbor && beetleDotNeighbor != beetleDot)
            {
                if (-beetleDotNeighbor.DirectionX == beetleDot.DirectionX && -beetleDotNeighbor.DirectionY == beetleDot.DirectionY)
                {
                    return false;
                }
            }
        }

        return true;

    }
    
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveBeetleDotsCommand));

        Dictionary<BeetleDot, Dot> dotsToSwap = new();
        foreach(Dot dot in board.Dots)
        {
            if(dot is BeetleDot beetleDot)
            {
                Dot dotToSwap = board.GetDotAt(beetleDot.Column + beetleDot.DirectionX, beetleDot.Row + beetleDot.DirectionY);
                if (CanMove(dotToSwap, beetleDot, board))
                {
                    dotsToSwap.TryAdd(beetleDot, dotToSwap);
                }
                else
                {
                    CoroutineHandler.StartStaticCoroutine(beetleDot.AlternateDirection());
                    
                }
                
            }
        }

        foreach(Dot dot in board.Dots)
        {
            if(dot is BeetleDot beetleDot)
            {
                if (dotsToSwap.TryGetValue(beetleDot, out var dotToSwap))
                {
                    int dotToSwapCol = dotToSwap.Column;
                    int dotToSwapRow = dotToSwap.Row;
                    int beetleDotCol = beetleDot.Column;
                    int beetleDotRow = beetleDot.Row;

                    CoroutineHandler.StartStaticCoroutine(DotController.MoveDot(beetleDot, dotToSwapCol, dotToSwapRow));
                    CoroutineHandler.StartStaticCoroutine(DotController.MoveDot(dotToSwap, beetleDotCol, beetleDotRow));
                    dotToSwap.Column = beetleDotCol;
                    dotToSwap.Row = beetleDotRow;
                    beetleDot.Column = dotToSwapCol;
                    beetleDot.Row = dotToSwapRow;
                    

                }

            }
            
        }


        yield return base.Execute(board);
    }
}
