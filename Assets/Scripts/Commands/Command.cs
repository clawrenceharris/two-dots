using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using static Type;
using System.Threading.Tasks;

public abstract class Command : ICommand
{
    public static Action<Command> onCommandExecuting;
    private List<Command> commandsExecuting = new();
    public bool DidExecute { get; protected set; }
    public abstract CommandType CommandType { get; }
    public Command NextCommand { get; private set; }

    public static event Action<Command> onCommandExecuted;
    public virtual IEnumerator Execute(Board board)
    {
        if (DidExecute)
        {
            onCommandExecuted?.Invoke(this);

        }
        yield return null;


    }

    public void SetNextCommand(Command command)
    {
        NextCommand = command;
    }
    protected void NotifyCommandExecuting(Command command)
    {
        if (commandsExecuting.Contains(command)){
            return;
        }
        commandsExecuting.Add(command);
        onCommandExecuted?.Invoke(command);
    }

}
