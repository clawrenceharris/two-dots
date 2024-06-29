using static Type;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExplodeCommand : Command
{
    private readonly IGrouping<ExplosionType, IExplodable> explodables;

    public override CommandType CommandType => CommandType.Bomb;
    public ExplodeCommand(IGrouping<ExplosionType, IExplodable> explodables)
    {
        this.explodables = explodables;
    }
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ExplodeCommand));
        List<IHittable> toHit = new();
        List<IEnumerator> coroutines = new();
        foreach (IExplodable explodable in explodables)
        {
            
            foreach (HitType hitType in explodable.ExplosionRules.Keys)
            {
                if (explodable.ExplosionRules.TryGetValue(hitType, out IExplosionRule rule))
                {
                    DidExecute = true;
                    toHit = rule.Validate(explodable, board);

                    Coroutine explosionCoroutine = CoroutineHandler.StartStaticCoroutine(
                    explodable.Explode(toHit, (hittable) => {
                       CoroutineHandler.StartStaticCoroutine(hittable.BombHit());
                        
                    }));


                }

            }
            foreach (HitType hitType in explodable.HitRules.Keys)
            {
                if (explodable.HitRules.TryGetValue(hitType, out var rule))
                {
                    if (rule.Validate(explodable, board))
                    {
                        CoroutineHandler.StartStaticCoroutine(explodable.Hit(hitType));
                    }
                }
            }
            //            if (explodable.HitCount >= explodable.HitsToClear)
            //{
            //   //CoroutineHandler.StartStaticCoroutine(explodable.Clear());
            //}
        }

        foreach (IHittable hittable in toHit)
        {
            CoroutineHandler.StartStaticCoroutine(hittable.Hit(explodables.First().HitType)); ;
        }
        yield return new WaitForSeconds(1f);

        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ExplodeCommand));
        }

        yield return base.Execute(board);
    }
}