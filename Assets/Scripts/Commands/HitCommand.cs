using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Represents a command that handles the hit detection and execution logic on the game board.
/// This command checks all IHittable elements on the board and applies hit logic on the element based its defined rules.
/// </summary>
public class HitCommand : Command
{

    public override CommandType CommandType => CommandType.Hit;

    private static int ongoingCoroutines = 0;

    /// <summary>
    /// Hits the given hittable object by first, validating whether the hittable 
    /// should be hit according to its hit validation rules, 
    /// then hiting both the desired hittable and any background 
    /// tile that is in the same position.
    /// </summary>
    /// <param name="hittable">The desired hittable object to hit</param>
    /// <param name="board">The game board</param>
    /// <param name="hitType">The type of hit to use</param>
    /// <remarks>
    /// If you need to do a hit on a hittable object without 
    /// checking for if the hit is valid, see <seealso cref="DoHitWithoutValidation"/>.
    /// </remarks>
    public static IEnumerator DoHitWithValidation(IHittable hittable, Board board, HitType hitType = HitType.None)
    {
        if(hittable == null)
        {
            yield break;
        }
            
        if (hittable.HitRule != null && hittable.HitRule.Validate(hittable, board))
        {
            yield return CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () => {
                //hit any normal tiles at the same position as the current hittable
                IBoardElement b = (IBoardElement)hittable;
                IHittable tile = board.GetTileAt<IHittable>(b.Column, b.Row);
                if(tile != null)
                    CoroutineHandler.StartStaticCoroutine(tile.Hit(hitType, null));
            }));                    
    
        }
    }


    /// <summary>
    /// Hits the given hittable object by hiting both the desired hittable 
    /// and any background tile that is in the same position. This method 
    /// skips validation of the  hittable object's hit rules
    /// </summary>
    /// <param name="hittable">The desired hittable object to hit</param>
    /// <param name="board">The game board</param>
    /// <param name="hitType">The type of hit to use</param>
    /// 
    public static IEnumerator DoHitWithoutValidation(IHittable hittable, Board board, HitType hitType = HitType.None)
    {
        if(hittable == null)
        {
            yield break;;
        }
            

        yield return CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () => {
            //hit any normal tiles at the same position as the current hittable
            IBoardElement b = (IBoardElement)hittable;
            IHittable tile = board.GetTileAt<IHittable>(b.Column, b.Row);
            if(tile != null){
                CoroutineHandler.StartStaticCoroutine(tile.Hit(hitType, null));
            }
                
        }));                    

        
    }


   
    public override IEnumerator Execute(Board board)
    {
        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(HitCommand));

        List<IHittable> dots = board.FindDotsOfType<IHittable>();
        List<IHittable> boardMechanicTiles = board.FindBoardMechanicTilesOfType<IHittable>();
        List<IHittable> hittables = dots.Concat(boardMechanicTiles).ToList();//all hittables on the board (excludes normal tiles)



        foreach (IHittable hittable in hittables)
        {
            ongoingCoroutines++;
            CoroutineHandler.StartStaticCoroutine(DoHitWithValidation(hittable, board, HitType.None),()=>{
                ongoingCoroutines--;
            });
            
        }

       



        //wait until all hit coroutines have finished
        yield return new WaitUntil(() => ongoingCoroutines == 0);

        yield return base.Execute(board);

    }
}





