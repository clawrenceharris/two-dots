using static Type;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExplodeCommand : Command
{
    public readonly IGrouping<ExplosionType, IExplodable> explodables;

    private readonly CommandType commandType;

    public override CommandType CommandType => commandType;

    public ExplodeCommand(IGrouping<ExplosionType, IExplodable> explodables, CommandType commandType)
    {
        this.explodables = explodables;
        this.commandType = commandType;
    }


    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ExplodeCommand));
        onCommandExecuting?.Invoke(this);
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
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () => hitCount++));

                    }));

                }

            }

            

        }

        yield return new WaitUntil(() => hitCount == hits.Count);



        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ExplodeCommand));
            CommandInvoker.Instance.Enqueue(new ClearCommand());
        }
        

        yield return base.Execute(board);
    }
}