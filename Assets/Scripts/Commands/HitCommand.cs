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
        List<IHittable> toClear = new();

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
                        yield return hittable.Hit(hitType);
                        toClear.Add(hittable);
                        if (hittable is IPreviewable previewable)
                        {
                            CoroutineHandler.StartStaticCoroutine(previewable.PreviewHit(hitType));
                            
                        }
                    }
                }

            }
            

        }
        yield return new WaitForSeconds(DotVisuals.hitDuration);
        foreach(IHittable hittable in toClear) {
            if (hittable.HitCount >= hittable.HitsToClear)
            {
                DidExecute = true;

                CoroutineHandler.StartStaticCoroutine(hittable.Clear());
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





