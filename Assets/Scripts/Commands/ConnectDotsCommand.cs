using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class ConnectDotsCommand : Command
{
    public override CommandType CommandType => CommandType.ConnectDots;


    
    public override IEnumerator Execute(Board board)
    {

        List<IHittable> dotsToHit = ConnectionManager.ToHit;



        foreach (IHittable hittable in dotsToHit)
        {

            DidExecute = true;
            CoroutineHandler.StartStaticCoroutine(hittable.Hit(HitType.Connection));
            if(hittable is IPreviewable previewable)
            {
              CoroutineHandler.StartStaticCoroutine(previewable.PreviewHit());
            }
        }

        yield return base.Execute(board);


    }

}
