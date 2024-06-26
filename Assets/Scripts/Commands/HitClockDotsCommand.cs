using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class HitClockDotsCommand : Command
{
    private LinkedList<ConnectableDot> connectedDots;

    public override CommandType CommandType => CommandType.HitClockDots;

    public HitClockDotsCommand(LinkedList<ConnectableDot> connectedDots)
    {
        this.connectedDots = connectedDots;
    }


    public override IEnumerator Execute(Board board)
    {
        foreach(ConnectableDot dot in connectedDots)
        {
            if(dot is ClockDot clockDot)
            {
                DidExecute = true;
                CoroutineHandler.StartStaticCoroutine(clockDot.Hit(HitType.Connection));
                if(clockDot.HitCount >= clockDot.HitsToClear)
                {
                    CoroutineHandler.StartStaticCoroutine(clockDot.Clear());

                }
            }
        }
        yield return base.Execute(board);
    }
}
