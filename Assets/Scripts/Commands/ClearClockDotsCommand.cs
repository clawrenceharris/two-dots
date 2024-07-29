using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ClearClockDotsCommand : Command
{
    public override CommandType CommandType => CommandType.ClearClockDots;
    public override IEnumerator Execute(Board board)
    {
        
        List<ClockDot> clockDots = board.FindDotsOfType<ClockDot>();
        int onGoingCorotuines = 0;
        foreach(ClockDot clockDot in clockDots)
        {
            if (clockDot.CurrentNumber == 0)
            {
                onGoingCorotuines++;
                CoroutineHandler.StartStaticCoroutine(clockDot.Clear(), () => onGoingCorotuines--) ;
            }
        }
        yield return new WaitUntil(() => onGoingCorotuines == 0);
        yield return base.Execute(board);
    }

}
