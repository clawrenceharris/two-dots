using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ExplodeGemsCommand : Command
{
    private readonly List<Gem> visited = new();
    private int ongoingCoroutines;
    public override CommandType CommandType => CommandType.GemExplode;
    private IEnumerator ExplodeGem(Gem gem, Board board, Action<List<IHittable>> onComplete = null)
    {
        List<IHittable> hittables = new();
        // Validate the explosion rule and get the list of elements to hit
        List<IHittable> toHit = gem.ExplosionRule.Validate(gem, board);
        hittables.AddRange(toHit);
        
        // Start hitting the gem
        yield return CoroutineHandler.StartStaticCoroutine(gem.Hit(HitType.GemExplosion, null));
        yield return new WaitForSeconds(0.5f);
        // Explode the gem and handle any additional gems that need to be exploded
        foreach(IHittable hittable in toHit){
            
            if(!visited.Contains(hittable) && hittable is Gem g){
                visited.Add(g);
                ongoingCoroutines++;
                CoroutineHandler.StartStaticCoroutine(ExplodeGem(g, board));
                
            }
            
        }

        yield return CoroutineHandler.StartStaticCoroutine(gem.Explode(toHit, board));
        ongoingCoroutines--;
        // Once done, triggers the onComplete callback
        onComplete?.Invoke(hittables);
    }

    public override IEnumerator Execute(Board board)
    {
        List<Coroutine> coroutines = new();
        List<Gem> gems = board.FindElementsOfType<Gem>();
        List<IHittable> toHit = new();
        List<IEnumerator> explodeGemCoroutines = new();
        foreach (Gem gem in gems)
        {
            if (gem.HitRule.Validate(gem, board) || gem.WasHit)
            {
                DidExecute = true;
                ongoingCoroutines++;
                CoroutineHandler.StartStaticCoroutine(ExplodeGemCoroutine(gem, board, toHit));
            }
        }

        yield return new WaitUntil(()=> ongoingCoroutines == 0);
        
        // After all explosions, hit all the remaining hittables
        foreach (IHittable hittable in toHit)
        {
            CoroutineHandler.StartStaticCoroutine(HitCommand.DoHitWithoutValidation(hittable, board, HitType.GemExplosion));     
        }
        yield return ClearCommand.DoClear(toHit.Distinct().ToList());
        yield return base.Execute(board);
    }

    /// <summary>
    /// Helper coroutine to explode a gem and collect hittables
    /// </summary>
    /// <param name="gem">The starting gem to explode</param>
    /// <param name="board">The game board</param>
    /// <param name="toHit">The hittable objects that the gem should hit</param>
    /// <returns></returns>
    private IEnumerator ExplodeGemCoroutine(Gem gem, Board board, List<IHittable> toHit)
    {
        List<IHittable> visited = new();
        yield return CoroutineHandler.StartStaticCoroutine(ExplodeGem(gem, board, collectedHittables =>
        {
            if (collectedHittables != null)
            {
                toHit.AddRange(collectedHittables);
            }
        }));
        
    }
}
