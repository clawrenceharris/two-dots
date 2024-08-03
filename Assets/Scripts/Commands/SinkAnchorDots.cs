using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkAnchorDotsCommand : Command
{
    public override CommandType CommandType => CommandType.SinkAnchorDots;

    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(SinkAnchorDotsCommand));

        List<AnchorDot> anchorDots = board.FindDotsOfType<AnchorDot>();

        foreach(AnchorDot anchorDot in anchorDots)
        {

            
            if(anchorDot.HitRule.Validate(anchorDot, board)){
                
                DidExecute = true;
                CoroutineHandler.StartStaticCoroutine(anchorDot.Hit(HitType.AnchorDot, null));

                
            
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
