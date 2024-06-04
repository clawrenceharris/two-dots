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
    public static bool IsExecuting { get; private set; }
    private bool commandsEnded = false; 
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
        IsExecuting = true; // Set flag to indicate that commands are executing
        yield return command.Execute(board); // Execute the command
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
        // Wait for a few seconds
        yield return new WaitForSeconds(3f); // Adjust the wait time as needed

        // Check if there are no new commands enqueued
        if (commands.Count == 0 && !IsExecuting)
        {
            if (!commandsEnded)
            {
                commandsEnded = true;
                Debug.Log("All commands have ended.");

                onCommandsEnded?.Invoke(); // Invoke the event when commands end

            }
        }
        else
        {
            commandsEnded = false;
        }
    }

    public bool CommandsEnded => commandsEnded; // Expose the commandsEnded field
}
