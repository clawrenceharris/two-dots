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

    public static void DoHit(IHittable hittable, Board board, HitType hitType = HitType.None)
    {
        if(hittable == null)
        {
            return;
        }
            
        if (hittable.HitRule != null && hittable.HitRule.Validate(hittable, board))
        {
            ongoingCoroutines++;
            CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () => {
                //hit any normal tiles at the same position as the current hittable
                IBoardElement b = (IBoardElement)hittable;
                IHittable tile = board.GetTileAt<IHittable>(b.Column, b.Row);
                if(tile != null)
                    CoroutineHandler.StartStaticCoroutine(tile.Hit(hitType, null));
            }),() => ongoingCoroutines--);                    
    
        }
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
  
            DoHit(hittable, board, HitType.None);
            
        }

       



        //wait until all hit coroutines have finished
        yield return new WaitUntil(() => ongoingCoroutines == 0);

        yield return base.Execute(board);

    }
}





