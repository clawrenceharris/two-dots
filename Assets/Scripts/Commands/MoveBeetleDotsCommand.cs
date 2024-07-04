using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using System.Linq;
public class MoveBeetleDotsCommand : MoveCommand
{
    private readonly Dictionary<BeetleDot, Dot> dotsToSwap = new();

    public override CommandType CommandType => CommandType.MoveBeetleDots;

    /// <summary>
    /// Changes the beetle dots direction
    /// if the current direction does not provide an available dot to swap
    /// </summary>
    /// <param name="beetleDot">The beetle dot whose direction is to be updated</param>
    /// <param name="dotToSwap">The dot that the beetle dot is directed towards</param>
    /// <param name="board">The game board</param>
    private void UpdateBeetleDotDirection(BeetleDot beetleDot, Dot dotToSwap, Board board)
    {
        if (!CanMove(dotToSwap))
        {
            Vector2Int newDir = FindBestDirection(beetleDot, board);
            beetleDot.ChangeDirection(newDir.x, newDir.y);
        }
       
    }

   
    public override bool CanMove(Dot targetDot)
    {
        //if the dot is null or it is a beetle 
        if(targetDot == null || targetDot is BeetleDot)
        {
            //then it is not avalaible to be swapped
            return false;
        }
        //if another beetle dot is already moving to this dot
        if (dotsToSwap.ContainsValue(targetDot))
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
        int beetleDotCount = 0;

        if (beetleDots.Where((dot) => !dot.WasHit).Count() > 0)
        {
            onCommandExecuting?.Invoke(this);

        }


        foreach (BeetleDot beetleDot in beetleDots)
        {
            
            Dot dotToSwap = board.Get<Dot>(beetleDot.Column + beetleDot.DirectionX, beetleDot.Row + beetleDot.DirectionY);

            //skip if the beetle dot was already hit
            if (beetleDot.WasHit)
            {
                //reset the flag for next move
                beetleDot.WasHit = false;
                continue;
            }
           
            //if the beetle dot can move and not hit
            if (CanMove(dotToSwap))
            {
                DidExecute = true;

                //then add it to the dictionary to be swapped
                dotsToSwap.TryAdd(beetleDot, dotToSwap);
            }
            else
            {
                //change the facing direction of the beetle
                CoroutineHandler.StartStaticCoroutine(beetleDot.TrySwap(() =>
                {
                    UpdateBeetleDotDirection(beetleDot, dotToSwap, board);

                }));
            }
           

        }

        foreach (BeetleDot beetleDot in dotsToSwap.Keys)
        {

            if (dotsToSwap.TryGetValue(beetleDot, out var dotToSwap))
            {

                int dotToSwapCol = dotToSwap.Column;
                int dotToSwapRow = dotToSwap.Row;
                int beetleDotCol = beetleDot.Column;
                int beetleDotRow = beetleDot.Row;

                CoroutineHandler.StartStaticCoroutine(beetleDot.DoSwap(dotToSwap, () =>
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


                }));

            }

     
        }

        yield return new WaitUntil(() => beetleDotCount == dotsToSwap.Count);


       
        if (DidExecute)
        {
            yield return new WaitForSeconds(0.7f);

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveBeetleDotsCommand));

        }

        yield return base.Execute(board);

    }
}
