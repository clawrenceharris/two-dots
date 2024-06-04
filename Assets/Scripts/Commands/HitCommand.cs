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

        List<IHittable> hittables = board.GetHittables();
        foreach (IHittable hittable in hittables)
        {
            if(hittable == null)
            {
                continue;
            }

            foreach (HitType hitType in hittable.HitRules.Keys)
            {
                if (hittable.HitRules.TryGetValue(hitType, out var rule))
                    if (rule.Validate(hittable, board) )
                    {

                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType));
                        DidExecute = true;

                    }
            }

        }

       

        yield return base.Execute(board);

    }
}





