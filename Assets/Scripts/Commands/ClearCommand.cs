using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ClearCommand : Command
{
    public override CommandType CommandType => CommandType.Clear;

    public static IEnumerator DoClear(List<IHittable> toClear){
        foreach (IHittable hittable in toClear)
        {
            
            if (hittable.HitCount >= hittable.HitsToClear)
            {
                CoroutineHandler.StartStaticCoroutine(hittable.Clear());
            }
        }
        yield return new WaitForSeconds(HittableVisuals.CLEAR_DURATION);
    }

    
    public static IEnumerator DoClear(IHittable hittable){
        if (hittable.HitCount >= hittable.HitsToClear){
            CoroutineHandler.StartStaticCoroutine(hittable.Clear());
            yield return new WaitForSeconds(HittableVisuals.CLEAR_DURATION);

        }
            
    }

    
    public override IEnumerator Execute(Board board)
    {
        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ClearCommand));


        List<IHittable> hittables = board.FindElementsOfType<IHittable>();
        List<IHittable> toClear = hittables.Where(hittable => hittable.HitCount >= hittable.HitsToClear).ToList();    
        
        DidExecute = toClear.Count > 0;
        yield return DoClear(toClear);

        Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ClearCommand));

        yield return base.Execute(board);

    }

}

