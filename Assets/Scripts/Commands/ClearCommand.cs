using System.Collections;
using UnityEngine;
using static Type;

public class ClearCommand : Command
{
    public override CommandType CommandType => CommandType.Clear;

    private void Clear(IHittable hittable)
    {
        if(hittable == null)
        {
            return;
        }
        
        if (hittable.HitCount >= hittable.HitsToClear)
        {
            DidExecute = true;
            hittable.Debug(Color.black);
            CoroutineHandler.StartStaticCoroutine(hittable.Clear());
        }

    }

   
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ClearCommand));

        for (int col = 0; col < board.Width; col++)
        {
            for (int row = 0; row < board.Height; row++)
            {
                IHittable hittable = board.GetHittableAt(col, row);
                
                Clear(hittable);

                
                
            }
        }



        yield return new WaitForSeconds(DotVisuals.defaultClearTime);

       


        yield return base.Execute(board);

    }
}
