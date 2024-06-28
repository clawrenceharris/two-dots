using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Type;





public class HitCommand : Command
{

    public override CommandType CommandType => CommandType.Hit;

   

    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(HitCommand));

        List<IHittable> hittables = board.GetElements<IHittable>();

        foreach (IHittable hittable in hittables)
        {
            if (hittable == null )
            {
                continue;
            }
          
            foreach (HitType hitType in hittable.HitRules.Keys)
            {
                if (hittable.HitRules.TryGetValue(hitType, out var rule))
                {
                    if (rule.Validate(hittable, board) && hittable is not IExplodable)
                    {
                        
                        DidExecute = true;

                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType));                        
                       
                    }
                }

            }
            

        }


        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(HitCommand));

            CommandInvoker.Instance.Enqueue(new ClearCommand());

        }
        else
        {
            CommandInvoker.Instance.Enqueue(new ExplosionCommand());

        }


        yield return base.Execute(board);

    }
}





