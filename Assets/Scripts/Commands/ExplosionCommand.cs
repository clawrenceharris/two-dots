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
        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(ExplosionCommand));

        List<IExplodable> explodables = board.FindElementsOfType<IExplodable>();
        if(explodables.Count == 0)
        {
            yield break;
        }
        var groupedExplodables = explodables
            .GroupBy(e => e.ExplosionType)
            .OrderBy(g => g.Key) // Ensures groups are processed based on ExplosionType priority
            .ToList();
        foreach (var group in groupedExplodables)
        {


            CommandInvoker.Instance.Enqueue(new ExplodeCommand(group, group.First().ExplosionType.ToCommandType()));
            
            DidExecute = true;
            
        }
       
        

        yield return base.Execute(board);


    }
}
