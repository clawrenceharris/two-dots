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
        int clears = 0;
        List<IHittable> toClear = new();
        foreach (IHittable hittable in hittables) {

            
            if (hittable != null && hittable.HitCount >= hittable.HitsToClear)
            {
                DidExecute = true;
                DotsGameObject dotsGameObject = (DotsGameObject)hittable;
                CoroutineHandler.StartStaticCoroutine(hittable.Clear((hittable) => clears++));
                toClear.Add(hittable);

            }
        }

        yield return new WaitForSeconds(HittableVisuals.defaultClearDuration);

        CommandInvoker.Instance.Enqueue(new BoardCommand());

        if (DidExecute)
        {

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ClearCommand));


        }
        else
        {
            CommandInvoker.Instance.Enqueue(new ExplosionCommand());

        }

       







        yield return base.Execute(board);

    }
}
