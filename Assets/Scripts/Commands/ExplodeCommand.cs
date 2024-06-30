using static Type;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExplodeCommand : Command
{
    private readonly IGrouping<ExplosionType, IExplodable> explodables;

    public override CommandType CommandType => CommandType.Explode;


    public ExplodeCommand(IGrouping<ExplosionType, IExplodable> explodables)
    {
        this.explodables = explodables;
    }


    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ExplodeCommand));

        List<IHittable> hits = new();
        int hitCount = 0;
        foreach (IExplodable explodable in explodables)
        {

            foreach (HitType hitType in explodable.ExplosionRules.Keys)
            {
                if (explodable.ExplosionRules.TryGetValue(hitType, out IExplosionRule rule))
                {
                    List<IHittable> toHit = rule.Validate(explodable, board);
                    DidExecute = true;
                    hits.AddRange(toHit);
                    CoroutineHandler.StartStaticCoroutine(
                    explodable.Explode(toHit, (hittable) =>
                    {
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType), () => hitCount++);

                    }));

                }

            }

            

        }

        yield return new WaitUntil(() => hitCount == hits.Count);



        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ExplodeCommand));
            CommandInvoker.Instance.Enqueue(new HitCommand());
        }
        

        yield return base.Execute(board);
    }
}