using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Type;
public class MoveClockDotsCommand : Command
{
    public override CommandType CommandType => CommandType.MoveClockDots;
    public static new bool DidExecute {get; private set;}
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveClockDotsCommand));

        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();

        for (int i = connectedDots.Count - 1; i >= 0; i--)
        {
            if (connectedDots[i] is ClockDot clockDot)
            {

                ConnectableDot lastEmptyDot = null;
                for (int k = i; k < connectedDots.Count; k++)
                {
                    if (connectedDots[k] is ClockDot)
                    {
                        continue;
                    }
                    lastEmptyDot = connectedDots[k];

                }
                yield return DotController.MoveDotThroughConnection(clockDot, lastEmptyDot);


            }

        }

        yield return new WaitForSeconds(0.7f);

        yield return base.Execute(board);
    }
}
