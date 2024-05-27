using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Type;



   

public class HitDotsCommand : Command
{
    public override CommandType CommandType => CommandType.HitDots;


    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(HitDotsCommand));

        List<IHittable> hittables = board.GetHittables();
        foreach (IHittable hittable in hittables)
        {
            if(hittable == null)
            {
                continue;
            }

            IEnumerable<HitType> keys = hittable.HitRules.Keys.Where(key => !Type.IsExplosion(key));
            foreach (HitType hitType in keys)
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





