using System.Collections;
using UnityEngine;
using static Type;


public class ClearCommand : Command
{
    public override CommandType CommandType => CommandType.Clear;


    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ClearCommand));


        for (int col = 0; col < board.Width; col++)
        {
            for(int row = 0; row < board.Height; row++)
            {
                IHittable hittable = board.Get<IHittable>(col, row);


                if (hittable != null &&  hittable.HitCount >= hittable.HitsToClear)
                {
                    DidExecute = true;
                    CoroutineHandler.StartStaticCoroutine(hittable.Clear());

                }
            }
        }
        yield return new WaitForSeconds(HittableVisuals.defaultClearDuration);

        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ClearCommand));

        }
        yield return base.Execute(board);

    }
}
