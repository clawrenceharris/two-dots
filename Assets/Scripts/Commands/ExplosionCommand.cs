using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;

public class ExplosionCommand : Command
{
    
    public override CommandType CommandType => CommandType.Explosion;

    
    public override IEnumerator Execute(Board board)
    {
        Dictionary<ExplosionType, List<IExplodable>> explodableMap = new();
        List<IExplodable> explodables = board.GetExplodables();
        List<IHittable> toClear = new();

        //exit method if there are no explodables to explode
        if (explodables.Count == 0)
        {
            yield break;
        }


        for (int i =0; i < explodables.Count; i++)
        {
            IExplodable explodable = explodables[i];
            foreach (HitType hitType in explodable.HitRules.Keys)
            {
                if (explodable.HitRules.TryGetValue(hitType, out var rule))
                {
                    if (rule.Validate(explodable, board))
                    {
                        if (!explodableMap.ContainsKey(explodable.ExplosionType))
                        {
                            explodableMap.Add(explodable.ExplosionType, new() { explodable });
                        }
                        else
                        {
                            explodableMap[explodable.ExplosionType].Add(explodable);
                        }
                    }
                }
                
            }
        }
        List<Coroutine> hitCoroutines = new();
        foreach(ExplosionType explosionType in explodableMap.Keys)
        {
            if(explodableMap.TryGetValue(explosionType, out var e))
            {

                foreach (IExplodable explodable in e)
                {

                    foreach (HitType hitType in explodable.ExplosionRules.Keys)
                    {

                        if (explodable.ExplosionRules.TryGetValue(hitType, out IExplosionRule rule))
                        {
                            DidExecute = true;

                            List<IHittable> toHit = rule.Validate(explodable, board);
                            CoroutineHandler.StartStaticCoroutine(explodable.Explode(toHit, (hittable) =>
                            {
                               hitCoroutines.Add(CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType)));
                                
                            }));
                            
                        }
                    }
                    
                    
                    CoroutineHandler.StartStaticCoroutine(explodable.Hit(explodable.HitType));
                    
                }
            }

            yield return new WaitForSeconds(DotVisuals.defaultClearDuration);



        }
        yield return new WaitForSeconds(DotVisuals.defaultClearDuration);

        if (DidExecute)
        {

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ExplosionCommand));

        }

        yield return base.Execute(board);


    }
}
