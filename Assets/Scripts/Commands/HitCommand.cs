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

    private int ongoingCoroutines = 0;

    private void Hit(IHittable hittable, Board board,Action onComplete = null)
    {
        if(hittable == null)
        {
            return;
        }
        foreach (HitType hitType in hittable.HitRules.Keys)
        {
            if (hittable.HitRules.TryGetValue(hitType, out var rule))
            {
                if (rule.Validate(hittable, board))
                {
                    ongoingCoroutines++;
                    CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () => {
                        onComplete?.Invoke();
                    }),() => ongoingCoroutines--);
                }
            }
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

            Hit(hittable, board, () =>
            {
                //hit any normal tiles at the same position as the current hittable
                IBoardElement b = (IBoardElement)hittable;
                IHittable tile = board.GetTileAt<IHittable>(b.Column, b.Row);
                Hit(tile, board);
            });
            
        }

       



        //wait until all hit coroutines have finished
        yield return new WaitUntil(() => ongoingCoroutines == 0);

        yield return base.Execute(board);

    }
}





