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

        List<IHittable> hittables = board.Get<IHittable>();

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
<<<<<<< HEAD
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType));
=======
                        if(hittable is Command command)
                            CommandInvoker.Instance.Enqueue(command);
>>>>>>> misc/fixes-and-refactoring
                        
                       
                    }
                }

            }
            

        }
       
        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(HitCommand));

        }

        yield return base.Execute(board);

    }
}





