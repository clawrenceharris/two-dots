using System.Collections;
using UnityEngine;
using System;
using static Type;


public abstract class Command : ICommand
{
    public bool DidExecute { get; protected set; }
    public abstract CommandType CommandType { get; }

    bool ICommand.DidExecute => DidExecute;

    public static event Action<Command> onCommandExecuted;
    public virtual IEnumerator Execute(Board board)
    {
        if (!DidExecute)
        {
            yield break;
        }
        yield return null;
        onCommandExecuted?.Invoke(this);


    }



}
