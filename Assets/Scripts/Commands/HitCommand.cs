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
        Debug.Log(nameof(HitCommand));

        List<IHittable> hittables = board.GetHittables();
        foreach (IHittable hittable in hittables)
        {
            if (hittable == null)
            {
                continue;
            }


            foreach (HitType hitType in hittable.HitRules.Keys)
            {
                if (hittable.HitRules.TryGetValue(hitType, out var rule))
                    if (rule.Validate(hittable, board))
                    {
                        DidExecute = true;
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType));
                        if (hittable.HitCount >= hittable.HitsToClear)
                        {
                            CoroutineHandler.StartStaticCoroutine(hittable.Clear());
                        }
                    }


                else if (new HitByExplosionRule().Validate(hittable, board))
                {
                    DidExecute = true;
                    CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType));
                    if (hittable.HitCount >= hittable.HitsToClear)
                    {
                        CoroutineHandler.StartStaticCoroutine(hittable.Clear());
                    }
                }


            }
        }
        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(HitCommand));

        }
        yield return new WaitForSeconds(DotVisuals.defaultClearDuration);

        yield return base.Execute(board);

    }
}





