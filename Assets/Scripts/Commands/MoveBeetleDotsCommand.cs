using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;
public class MoveBeetleDotsCommand : Command
{
    Dictionary<BeetleDot, Dot> dotsToSwap = new();

    public override CommandType CommandType => CommandType.MoveBeetleDots;




    /// <summary>
    /// Finds the best direction that the beetle dot can turn to in which the
    /// dot it is directed towards can be moved to
    /// </summary>
    /// <param name="dot"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private Vector2Int FindBestDirection(BeetleDot dot, Board board)
    {

        int rightX = -dot.DirectionY ;
        int rightY = dot.DirectionX;
        int leftX = dot.DirectionY;
        int leftY = -dot.DirectionX;

        //get the dot that is 90 degrees to the left of the beetle dot (y, -x)
        Dot left = board.GetDotAt(leftX + dot.Column, leftY + dot.Row);
       
        if (CanMove(left))
        {
            return new Vector2Int(leftX, leftY);
        }
        else 
        {
            return new Vector2Int(rightX, rightY);

        }

    }


    /// <summary>
    ///
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
            CoroutineHandler.StartStaticCoroutine(dot.ChangeDirection(newDir.x, newDir.y));
        }
       
    }

    /// <summary>
    /// Retutns whether or not a beetle dot
    /// can swap positions with the given dot to swap.
    /// </summary>
    /// <param name="dotToSwap">The dot that the beetle should swap positions with</param>
    /// <returns></returns>
    private bool CanMove(Dot dotToSwap)
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


    /// <summary>
    /// Returns whether the beetle dot wants to move,
    /// meaning it wasnt hit already in the last move but it can not move forward
    /// </summary>
    /// <param name="beetleDot">The beetle dot</param>
    /// <param name="dotToSwap">The dot to be swapped with</param>
    /// <returns></returns>
    private bool WantsToMove(BeetleDot beetleDot, Dot dotToSwap)
    {
        return !beetleDot.WasHit && !CanMove(dotToSwap);
    }

    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveBeetleDotsCommand));

        for(int i = 0; i < board.Width; i++)

        {
            for(int j = 0; j < board.Height; j++)
            {
                Dot dot = board.GetDotAt(i, j);
                if (dot is BeetleDot beetleDot)
                {
                    Dot dotToSwap = board.GetDotAt(beetleDot.Column + beetleDot.DirectionX, beetleDot.Row + beetleDot.DirectionY);

                    //if the dot can move and wants to move 
                    if (CanMove(dotToSwap))
                    {
                        //then add it to the dictionary
                        dotsToSwap.TryAdd(beetleDot, dotToSwap);

                    }
                    else
                    {
                        //visually try to make the swap
                        CoroutineHandler.StartStaticCoroutine(beetleDot.VisualController.TrySwap(() =>
                        {
                            UpdateBeetleDotDirection(beetleDot, dotToSwap, board);

                        }));

                    }


                }
            }
            
        }

        for (int i = 0; i < board.Width; i++)

        {
            for (int j = 0; j < board.Height; j++)
            {
                Dot dot = board.GetDotAt(i, j);
                if (dot is BeetleDot beetleDot)
                {
                    if (dotsToSwap.TryGetValue(beetleDot, out var dotToSwap))
                    {
                        int dotToSwapCol = dotToSwap.Column;
                        int dotToSwapRow = dotToSwap.Row;
                        int beetleDotCol = beetleDot.Column;
                        int beetleDotRow = beetleDot.Row;
                        CoroutineHandler.StartStaticCoroutine(beetleDot.DoSwap(dotToSwap, () =>
                        {

                            board.MoveDot(dotToSwap, beetleDotCol, beetleDotRow);
                            board.MoveDot(beetleDot, dotToSwapCol, dotToSwapRow);

                            Dot nextDotToSwap = board.GetDotAt(dotToSwapCol + beetleDot.DirectionX, dotToSwapRow + beetleDot.DirectionY);

                            //beetle dot moved so update direction
                            UpdateBeetleDotDirection(beetleDot, nextDotToSwap, board);


                        }));

                    }
                }
            }
            
        }

        


        yield return base.Execute(board);
    }
}
