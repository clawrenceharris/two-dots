using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Represents a command that handles the explosion logic for a group explodables of the same type.
/// This command iterates through all IExplodable elements, applies explosion effects based on defined rules,
/// and triggers subsequent hit effects on affected elements.
/// </summary>
public class ExplodeCommand : Command
{
    /// <summary>
    /// A grouping of IExplodable elements by their ExplosionType.
    /// </summary>
    public readonly IGrouping<ExplosionType, IExplodable> explodables;

   
    private readonly CommandType commandType;

    public override CommandType CommandType => commandType;

    public ExplodeCommand(IGrouping<ExplosionType, IExplodable> explodables, CommandType commandType)
    {
        this.explodables = explodables;
        this.commandType = commandType;
    }


    /// <summary>
    /// Executes the explode command on the specified board.
    /// This method iterates through all IExplodable elements, applies explosion effects based on explosion rules,
    /// triggers hit effects on affected elements, and enqueues appropriate follow-up commands.
    /// </summary>
    /// <param name="board">The game board on which to execute the command.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ExplodeCommand));

        List<IHittable> hittables = new();// List to store elements that were hit
        List<Coroutine> coroutines = new();
        int hitCount = 0;

        onCommandExecuting?.Invoke(this);

        foreach (IExplodable explodable in explodables)
        {
            foreach (HitType hitType in explodable.ExplosionRules.Keys)
            {
                if (explodable.ExplosionRules.TryGetValue(hitType, out IExplosionRule rule))
                {
                    // Validate the explosion rule and get the list of elements to hit
                    List<IHittable> toHit = rule.Validate(explodable, board);
                    DidExecute = true;

                    // Add the elements to the hit list
                    hittables.AddRange(toHit.Where(hittable => hittable != null));

                    // Start the coroutine to apply the explosion effect and subsequent hit effects


                    if(explodable == explodables.Last())
                        CoroutineHandler.StartStaticCoroutine(
                        explodable.Explode(hittables, (hittable) =>
                        {
                            if(hittable != null)
                                CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () =>
                                {
                                    hitCount++;
                                    //hit any normal tiles at the same position as the current hittable
                                    IBoardElement b = (IBoardElement)hittable;
                                    IHittable tile = board.GetTileAt<IHittable>(b.Column, b.Row);
                                    if(tile != null)
                                    {
                                        CoroutineHandler.StartStaticCoroutine(tile.Hit(hitType));

                                    }


                                }));

                        }));

                    


                }

                
           

            }
        }
        

        // Wait until all hit effects have been processed
        yield return new WaitUntil(() => hitCount == hittables.Count);



        

        yield return base.Execute(board);
    }
}