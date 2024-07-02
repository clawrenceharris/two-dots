using System.Collections;
using UnityEngine;
using static Type;
using System.Collections.Generic;


public class ClearCommand : Command
{
    public override CommandType CommandType => CommandType.Clear;


    public override IEnumerator Execute(Board board)
    {
        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ClearCommand));


        List<IHittable> hittables = board.GetElements<IHittable>();

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

        CommandInvoker.Instance.Enqueue(new BoardCommand());



        yield return base.Execute(board);

    }
}

