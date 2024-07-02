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
        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(HitCommand));
        List<IHittable> hits = new();
        List<IHittable> hittables = board.GetElements<IHittable>();
        int hitCount = 0;
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
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () => hitCount++));
                    }


                }

            }
            


        }
        yield return new WaitUntil(() => hits.Count == hitCount);


        if (DidExecute)
        {
            CommandInvoker.Instance.Enqueue(new ClearCommand());

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(HitCommand));


        }
        else
        {
            CommandInvoker.Instance.Enqueue(new ExplosionCommand());

        }


        yield return base.Execute(board);

    }
}





