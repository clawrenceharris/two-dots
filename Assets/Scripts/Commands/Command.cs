using System.Collections;
using UnityEngine;
using System;
using static Type;


public abstract class Command : MonoBehaviour, ICommand
{
    public bool DidExecute { get; protected set; }
    public abstract CommandType CommandType { get; }

    bool ICommand.DidExecute => DidExecute;

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
