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
        List<IEnumerator> hitCoroutines = new();
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(HitCommand));
        List<IHittable> hits = new();
        List<IHittable> hittables = board.GetElements<IHittable>();

        foreach (IHittable hittable in hittables)
        {
            if (hittable == null)
            {
                continue;
            }
           
            foreach (HitType hitType in hittable.HitRules.Keys)
            {
                if (hittable.HitRules.TryGetValue(hitType, out var rule))
                {
                    if (rule.Validate(hittable, board))
                    {
                        
                        DidExecute = true;
                        hits.Add(hittable);
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType));
                    }


                }

            }
            if (hittable != null && hittable.HitCount >= hittable.HitsToClear)
            {
                DidExecute = true;
                DotsGameObject dotsGameObject = (DotsGameObject)hittable;
                CoroutineHandler.StartStaticCoroutine(hittable.Clear());

            }


        }

        yield return new WaitForSeconds(HittableVisuals.hitDuration);

        if (DidExecute)
        {
            // CommandInvoker.Instance.Enqueue(new ClearCommand());
            CommandInvoker.Instance.Enqueue(new BoardCommand());

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(HitCommand));


        }
        else
        {
            CommandInvoker.Instance.Enqueue(new ExplosionCommand());

        }


        yield return base.Execute(board);

    }
}





