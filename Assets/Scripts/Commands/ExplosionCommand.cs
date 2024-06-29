using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;
using System.Linq;

public class ExplosionCommand : Command
{
    
    public override CommandType CommandType => CommandType.Explosion;


    
    public override IEnumerator Execute(Board board)
    {
        CommandInvoker.PriorityQueue<ICommand> explodableQueue = new();
        
        List<IExplodable> explodables = board.GetExplodables();
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ExplosionCommand));

        var groupedExplodables = explodables
            .GroupBy(e => e.ExplosionType)
            .OrderBy(g => g.Key) // Ensures groups are processed based on ExplosionType priority
            .ToList();
        foreach (var group in groupedExplodables)
        {
            
            CommandInvoker.Instance.Enqueue(new ExplodeCommand(group));
            
            DidExecute = true;
            
        }
       
        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(ExplosionCommand));
            CommandInvoker.Instance.Enqueue(new ClearCommand());
        }

        yield return base.Execute(board);


    }
}
