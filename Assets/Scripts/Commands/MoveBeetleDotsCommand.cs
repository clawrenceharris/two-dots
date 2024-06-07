using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class MoveBeetleDotsCommand : Command
{
    Dictionary<BeetleDot, Dot> dotsToSwap = new();

    public override CommandType CommandType => CommandType.MoveBeetleDots;

    private bool CanMove(Dot dotToSwap)
    {

        if(dotToSwap == null || dotToSwap is BeetleDot)
        {
            return false;
        }
        if (dotsToSwap.ContainsValue(dotToSwap))
        {
            return false;
        }
        return true;

    }
    
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveBeetleDotsCommand));

        foreach(Dot dot in board.Dots)
        {
            if(dot is BeetleDot beetleDot)
            {
                Dot dotToSwap = board.GetDotAt(beetleDot.Column + beetleDot.DirectionX, beetleDot.Row + beetleDot.DirectionY);
                if (CanMove(dotToSwap))
                {
                    dotsToSwap.TryAdd(beetleDot, dotToSwap);
                }
                else
                {
                    Debug.Log("Changing direction!");
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
                    //dotToSwap.Column = beetleDotCol;
                    //dotToSwap.Row = beetleDotRow;
                    //beetleDot.Column = dotToSwapCol;
                    //beetleDot.Row = dotToSwapRow;
                    

                }

            }
            
        }


        yield return base.Execute(board);
    }
}
