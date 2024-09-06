using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ClearClockDotsCommand : Command
{
    public override CommandType CommandType => CommandType.ClearClockDots;
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ClearClockDotsCommand));
        List<ClockDot> clockDots = board.FindDotsOfType<ClockDot>();
        foreach(ClockDot clockDot in clockDots)
        {
            if (clockDot.CurrentNumber == 0)
            {
                DidExecute = true;
                CoroutineHandler.StartStaticCoroutine(clockDot.Clear());
            }
        }
        if(DidExecute){
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ClearClockDotsCommand));
        }
        yield return base.Execute(board);
    }

}
