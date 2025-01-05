using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Represents a command that handles the movement logic of beetle dots on the game board.
/// This command checks each beetle dot's current direction, updates the direction if needed, and
/// performs the swap with the target dot if possible.
/// </summary>
public class MoveBeetleDotsCommand : MoveCommand
{
    //Dictionary that maps each movable beetle dot to a dot to swap 
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
        // If the dot is null or it is a beetle dot, it is not available to be swapped
        if (targetDot == null || targetDot is BeetleDot)
        {
            return false;
        }
        // If another beetle dot is already moving to this dot, it is not available to be swapped
        if (dotsToSwap.ContainsValue(targetDot))
        {
            return false;
        }
        return true;

    }


    /// <summary>
    /// Executes the move command for beetle dots on the specified board.
    /// This method iterates through all beetle dots, updates their direction if necessary,
    /// and performs the swap with the target dot if possible.
    /// </summary>
    /// <param name="board">The game board on which to execute the command.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveBeetleDotsCommand));

        List<BeetleDot> beetleDots = board.FindElementsOfType<BeetleDot>();
        int beetleDotCount = 0;
        int ongoingCoroutines = 0;

        // Perform the swap for each beetle dot in the dictionary
        foreach (BeetleDot beetleDot in beetleDots)
        {
            
            Dot dotToSwap = beetleDot.GetTarget(board);

            ////skip if the beetle dot was already hit
            if (beetleDot.WasHit)
            {
               //reset the flag for next move
               beetleDot.WasHit = false;
               continue;
            }
           
            //if the beetle dot can move and not hit
            
                NotifyCommandExecuting(this);
                DidExecute = true;
                ongoingCoroutines++;

                CoroutineHandler.StartStaticCoroutine(beetleDot.TrySwap(board, (result)=>{
                    
                    if(result == false){
                        return;
                    }
                    ongoingCoroutines--;
                    int dotToSwapCol = dotToSwap.Column;
                    int dotToSwapRow = dotToSwap.Row;
                    int beetleDotCol = beetleDot.Column;
                    int beetleDotRow = beetleDot.Row;
                    board.Put(dotToSwap, beetleDotCol, beetleDotRow);
                    board.Put(beetleDot, dotToSwapCol, dotToSwapRow);

                    dotToSwap.Column = beetleDotCol;
                    dotToSwap.Row = beetleDotRow;
                    beetleDot.Column = dotToSwapCol;
                    beetleDot.Row = dotToSwapRow;



                
                }));

            
            
          

              
            
           

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
