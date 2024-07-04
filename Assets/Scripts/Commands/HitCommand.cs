using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Type;

/// <summary>
/// Represents a command that handles the hit detection and execution logic on the game board.
/// This command checks all IHittable elements on the board and applies hit logic on the element based its defined rules.
/// If any hits are executed, it enqueues a ClearCommand to clear the board.
/// If no hits are executed, it enqueues an ExplosionCommand.
/// </summary>
public class HitCommand : Command
{

    public override CommandType CommandType => CommandType.Hit;

   

    public override IEnumerator Execute(Board board)
    {
        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(HitCommand));
        List<IHittable> hits = new();
        List<IHittable> hittables = board.GetElements<IHittable>();
        int hitCount = 0;
        foreach (IHittable hittable in hittables)
        {
            if (hittable == null)
            {
                continue;
            }
           
            foreach (HitType hitType in hittable.HitRules.Keys)
            {
                if (hittable.HitRules.TryGetValue(hitType, out var rule))
                {
                    if (rule.Validate(hittable, board))
                    {
                        
                        DidExecute = true;
                        hits.Add(hittable);
                        CoroutineHandler.StartStaticCoroutine(hittable.Hit(hitType, () => hitCount++));
                    }


                }

            }
            


        }
        yield return new WaitUntil(() => hits.Count == hitCount);


        if (DidExecute)
        {
            CommandInvoker.Instance.Enqueue(new ClearCommand());

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(HitCommand));


        }
        else
        {
            CommandInvoker.Instance.Enqueue(new ExplosionCommand());

        }


        yield return base.Execute(board);

    }
}





