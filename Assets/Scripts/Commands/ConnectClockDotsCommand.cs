using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class ConnectClockDotsCommand 
{

    //public override CommandType CommandType => CommandType.HitClockDots;
    
   
    //public override IEnumerator Execute(Board board)
    //{

    //    List<IHittable> dotsToHit = ConnectionManager.ToHit;


    //    foreach (Dot dot in board.Dots)
    //    {

    //        if (dot is not ClockDot clockDot)
    //        {
    //            continue;
    //        }
    //        if (dotsToHit.Contains(clockDot))
    //        {
    //            DidExecute = true;

    //            CoroutineHandler.StartStaticCoroutine(clockDot.Hit(HitType.ClockDot));

    //        }
    //        else
    //        {
    //            clockDot.UpdateNumber(clockDot.CurrentNumber);

    //        }







    //    }

    //    yield return base.Execute(board);


    //}

    
}
