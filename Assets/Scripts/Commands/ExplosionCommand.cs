using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using static Type;

public class ExplosionCommand : Command
{
    
    public override CommandType CommandType => CommandType.Explosion;


    
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ExplosionCommand));

        List<IExplodable> explodables = board.GetExplodables();

        if (explodables.Count == 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.7f);

        
        for(int i =0; i < explodables.Count; i++)
        {
            IExplodable explodable = explodables[i];
            if (explodable == null)
            {
                continue;
            }

            foreach (HitType hitType in explodable.ExplosionRules.Keys)
            {
                if (explodable.ExplosionRules.TryGetValue(hitType, out IExplosionRule rule))
                {
                    List<IHittable> toHit = rule.Validate(explodable, board);

                    foreach (IHittable hittable in toHit)
                    {
                        
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType));
                        DidExecute = true;

                    }

                 
                    CoroutineHandler.StartStaticCoroutine(explodable.Explode(toHit));
                    CoroutineHandler.StartStaticCoroutine(explodable.Hit(hitType));


                   



                }



            }

        }
        









        yield return new WaitForSeconds(0.5f);


        if (DidExecute)
        {
            CommandInvoker.Instance.Enqueue(new ClearCommand());

        }
        yield return base.Execute(board);


    }
}