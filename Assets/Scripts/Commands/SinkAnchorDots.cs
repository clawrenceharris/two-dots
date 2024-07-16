using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class SinkAnchorDotsCommand : Command
{
    public override CommandType CommandType => CommandType.SinkAnchorDots;

    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(SinkAnchorDotsCommand));

        List<AnchorDot> anchorDots = board.FindDotsOfType<AnchorDot>();

        foreach(AnchorDot anchorDot in anchorDots)
        {

            foreach(HitType hitType in anchorDot.HitRules.Keys)
            {
                if(anchorDot.HitRules.TryGetValue(hitType, out var rule)){
                    if(rule.Validate(anchorDot, board))
                    {
                        DidExecute = true;
                        CoroutineHandler.StartStaticCoroutine(anchorDot.Hit(hitType, null));

                    }
                }
            }
        }
        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(SinkAnchorDotsCommand));

            onCommandExecuting?.Invoke(this);
        }
        yield return base.Execute(board);
    }
}
