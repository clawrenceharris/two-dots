using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ExplodeGemsCommand : Command
{
    public override CommandType CommandType => CommandType.GemExplode;
    private IEnumerator ExplodeGem(Gem gem, Board board, Action<List<IHittable>> onComplete = null)
    {
        List<IHittable> hittables = new();
        // Validate the explosion rule and get the list of elements to hit
        List<IHittable> toHit = gem.ExplosionRule.Validate(gem, board);
        hittables.AddRange(toHit);
        
        // Start hitting the gem
        CoroutineHandler.StartStaticCoroutine(gem.Hit(HitType.GemExplosion, null));

        // Explode the gem and handle any additional gems that need to be exploded
        
        yield return CoroutineHandler.StartStaticCoroutine(gem.Explode(toHit, board, hittable =>
        {
            if (hittable is Gem gem)
            {
                // Hit the current gem
                HitCommand.DoHitWithoutValidation(hittable, board, HitType.GemExplosion);

                // Recursively explode other gems
                
                CoroutineHandler.StartStaticCoroutine(ExplodeGem(gem, board));
            }
        }));
        
        // Once done, trigger the onComplete callback (if provided)
        onComplete?.Invoke(hittables);
    }

    public override IEnumerator Execute(Board board)
    {
        List<Coroutine> coroutines = new();
        List<Gem> gems = board.FindElementsOfType<Gem>();
        List<IHittable> toHit = new();
        List<IEnumerator> explodeGemCoroutines = new List<IEnumerator>();
        foreach (Gem gem in gems)
        {
            if (gem.HitRule.Validate(gem, board) || gem.WasHit)
            {
                DidExecute = true;
                yield return ExplodeGemCoroutine(gem, board, toHit);
            }
        }

       

        // After all explosions, hit all the remaining hittables
        foreach (IHittable hittable in toHit)
        {
            CoroutineHandler.StartStaticCoroutine(HitCommand.DoHitWithoutValidation(hittable, board, HitType.GemExplosion),()=>{
                ClearCommand.DoClear(hittable);
            });       
        }
        
        yield return base.Execute(board);
    }

    // Helper coroutine to explode a gem and collect hittables
    private IEnumerator ExplodeGemCoroutine(Gem gem, Board board, List<IHittable> toHit)
    {
        List<IHittable> visited = new();
        yield return ExplodeGem(gem, board, collectedHittables =>
        {
            if (collectedHittables != null)
            {
                toHit.AddRange(collectedHittables.Distinct());
            }
        });
        
    }
}
