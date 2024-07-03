using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using System.Linq;
public class MoveBeetleDotsCommand : MoveCommand
{
    readonly Dictionary<BeetleDot, Dot> dotsToSwap = new();

    public override CommandType CommandType => CommandType.MoveBeetleDots;

    /// <summary>
    /// Changes the beetle dots direction
    /// if the current direction does not provide an available dot to swap
    /// </summary>
    /// <param name="dot">The beetle dot whose direction is to be updated</param>
    /// <param name="dotToSwap">The dot that the beetle dot is directed towards</param>
    /// <param name="board">The game board</param>
    private void UpdateBeetleDotDirection(BeetleDot dot, Dot dotToSwap, Board board)
    {
        if (!CanMove(dotToSwap))
        {
            Vector2Int newDir = FindBestDirection(dot, board);
            dot.ChangeDirection(newDir.x, newDir.y);
        }
       
    }

   
    public override bool CanMove(Dot dotToSwap)
    {
        //if the dot is null or it is a beetle 
        if(dotToSwap == null || dotToSwap is BeetleDot)
        {
            //then it is not avalaible to be swapped
            return false;
        }
        //if a beetle dot is already paired with this dot
        if (dotsToSwap.ContainsValue(dotToSwap))
        {
            //then it is not avalaible to be swapped
            return false;
        }
        return true;

    }



    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveBeetleDotsCommand));

        List<BeetleDot> beetleDots = board.GetElements<BeetleDot>();
        beetleDots = beetleDots.Where((dot) => !dot.WasHit).ToList();

        int beetleDotCount = 0;
        foreach (BeetleDot beetleDot in beetleDots)
        {
            
            Dot dotToSwap = board.Get<Dot>(beetleDot.Column + beetleDot.DirectionX, beetleDot.Row + beetleDot.DirectionY);

            //if the beetle dot can move
            if (CanMove(dotToSwap))
            {
                DidExecute = true;
                onCommandExecuting?.Invoke(this);

                //then add it to the dictionary
                dotsToSwap.TryAdd(beetleDot, dotToSwap);
            }
            else
            {
                //change the facing direction of the beetle
                CoroutineHandler.StartStaticCoroutine(beetleDot.TrySwap(), () =>
                {
                    UpdateBeetleDotDirection(beetleDot, dotToSwap, board);

                });
            }
                
        }

        foreach (BeetleDot beetleDot in beetleDots)
        {

            if (dotsToSwap.TryGetValue(beetleDot, out var dotToSwap))
            {

                int dotToSwapCol = dotToSwap.Column;
                int dotToSwapRow = dotToSwap.Row;
                int beetleDotCol = beetleDot.Column;
                int beetleDotRow = beetleDot.Row;
                CoroutineHandler.StartStaticCoroutine(beetleDot.DoSwap(dotToSwap), () =>
                {

                    beetleDotCount++;

                    board.Put(dotToSwap, beetleDotCol, beetleDotRow);
                    board.Put(beetleDot, dotToSwapCol, dotToSwapRow);

                    dotToSwap.Column = beetleDotCol;
                    dotToSwap.Row = beetleDotRow;
                    beetleDot.Column = dotToSwapCol;
                    beetleDot.Row = dotToSwapRow;

                    Dot nextDotToSwap = board.Get<Dot>(dotToSwapCol + beetleDot.DirectionX, dotToSwapRow + beetleDot.DirectionY);

                    //beetle dot moved so update direction
                    UpdateBeetleDotDirection(beetleDot, nextDotToSwap, board);


                });

            }
     
        }

        yield return new WaitUntil(() => beetleDotCount == dotsToSwap.Count);

        if (DidExecute)
        {

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveBeetleDotsCommand));

        }

        yield return base.Execute(board);

    }
}
