using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ClearCommand : Command
{
    public override CommandType CommandType => CommandType.Clear;


    public override IEnumerator Execute(Board board)
    {
        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ClearCommand));


        List<IHittable> hittables = board.FindElementsOfType<IHittable>();

        foreach (IHittable hittable in hittables)
        {
            if (hittable != null && hittable.HitCount >= hittable.HitsToClear)
            {
                DidExecute = true;
                

                CoroutineHandler.StartStaticCoroutine(hittable.Clear());

            }
        }


        yield return new WaitForSeconds(HittableVisuals.defaultClearDuration);




        Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ClearCommand));




        yield return base.Execute(board);

    }

}

