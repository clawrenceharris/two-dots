using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    private readonly Queue<Command> commands = new();
    private readonly Board board;
    public static CommandInvoker Instance;
    public static int commandCount;
    public static bool CommandsEnded { get; private set; } = true;
    private Coroutine checkCommandsEndedCoroutine;
    public static event Action onCommandsEnded;

    public CommandInvoker(Board board)
    {
        Instance = this;
        Command.onCommandExecuted += OnCommandExecuted;
        this.board = board;
    }

    public void Enqueue(Command command)
    {
        CommandsEnded = false;
        commands.Enqueue(command);

        // Restart the check coroutine when a new command is executed
        RestartCheckCommandsEndedCoroutine();

    }

    public void ExecuteNextCommand()
    {
        if (commands.Count > 0)
        {
            Command command = commands.Dequeue();
            CoroutineHandler.StartStaticCoroutine(ExecuteCommandCo(command));
        }
    }

    private IEnumerator ExecuteCommandCo(Command command)
    {
        yield return command.Execute(board); 
        ExecuteNextCommand();
    }

    private void OnCommandExecuted(Command command)
    {
        commandCount++;
    }

    
    private void RestartCheckCommandsEndedCoroutine()
    {
        if (checkCommandsEndedCoroutine != null)
        {
           CoroutineHandler.StopStaticCoroutine(checkCommandsEndedCoroutine);
        }
        checkCommandsEndedCoroutine = CoroutineHandler.StartStaticCoroutine(CheckCommandsEnded());
    }

    private IEnumerator CheckCommandsEnded()
    {
       
        yield return new WaitForSeconds(1f); 

        if (commands.Count == 0  && !CommandsEnded)
        {
            
                CommandsEnded = true;
                Debug.Log(commandCount+ " All commands have ended.");

                onCommandsEnded?.Invoke();
        }
        
    }

}
