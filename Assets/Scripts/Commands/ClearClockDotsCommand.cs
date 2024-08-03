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
        foreach(ClockDot clockDot in clockDots)
        {
            if (clockDot.CurrentNumber == 0)
            {
                CoroutineHandler.StartStaticCoroutine(clockDot.Clear());
            }
        }
        yield return base.Execute(board);
    }

}
