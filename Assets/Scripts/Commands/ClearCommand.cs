using System.Collections;
using UnityEngine;
using static Type;
using System.Collections.Generic;


public class ClearCommand : Command
{
    public override CommandType CommandType => CommandType.Clear;


    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ClearCommand));



        List<IHittable> hittables = board.GetElements<IHittable>();

        foreach (IHittable hittable in hittables) {
            if (hittable != null && hittable.HitCount >= hittable.HitsToClear)
            {
                DidExecute = true;
                CoroutineHandler.StartStaticCoroutine(hittable.Clear());

            }
        }

        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ClearCommand));
            CommandInvoker.Instance.Enqueue(new BoardCommand());

            yield return new WaitForSeconds(HittableVisuals.defaultClearDuration);

        }

        else
        {
            CommandInvoker.Instance.Enqueue(new ExplosionCommand());

        }







        yield return base.Execute(board);

    }
}
