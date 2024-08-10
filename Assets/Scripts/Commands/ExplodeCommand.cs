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
    public readonly List<IExplodable> explodables;

   
    private readonly CommandType commandType;

    public override CommandType CommandType => commandType;

    public ExplodeCommand(List<IExplodable> explodables, CommandType commandType)
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
        int ongoingCoroutines = 0;

        onCommandExecuting?.Invoke(this);

        foreach (IExplodable explodable in explodables)
        {
            
                
            // Validate the explosion rule and get the list of elements to hit
            List<IHittable> toHit = explodable.ExplosionRule.Validate(explodable, board);
            DidExecute = true;

            // Add the elements to the hit list
            hittables.AddRange(toHit.Where(hittable => hittable != null));

            // Start the coroutine to apply the explosion effect and subsequent hit effects


            if(explodable == explodables.Last()){

                ongoingCoroutines++;
                CoroutineHandler.StartStaticCoroutine(
                explodable.Explode(hittables, (hittable) =>
                {
                    if(hittable != null)
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(HitType.Explosion, () =>
                        {
                            //hit any background tiles at the same position as the current hittable
                            IBoardElement b = (IBoardElement)hittable;

                            IHittable tile = board.GetTileAt<IHittable>(b.Column, b.Row);
                            if(tile != null)
                            {
                                //hit the tile once if the hittable takes one hit to 
                                // clear in one hit and twice if it takes more than one hit to clear
                                CoroutineHandler.StartStaticCoroutine(tile.Hit(HitType.Explosion,() =>{
                                    if(hittable is Dot dot && !dot.DotType.IsBasicDot()){
                                        CoroutineHandler.StartStaticCoroutine(tile.Hit(HitType.DotHit));
                                    }
                                }));
                               

                                
                            
                            }
                        

                        }));

                }),()=>ongoingCoroutines--); 

            }
        }
        

        // Wait until all hit effects have been processed
        yield return new WaitUntil(() => ongoingCoroutines == 0);



        

        yield return base.Execute(board);
    }
}