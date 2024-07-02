using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using static Type;


public abstract class Command : ICommand
{
    public static Action<Command> onCommandExecuting;

    public bool DidExecute { get; protected set; }
    public abstract CommandType CommandType { get; }

    public static event Action<Command> onCommandExecuted;
    public virtual IEnumerator Execute(Board board)
    {
        if (DidExecute)
        {
            onCommandExecuted?.Invoke(this);
            DidExecute = false;

        }
        yield return null;


    }



}
