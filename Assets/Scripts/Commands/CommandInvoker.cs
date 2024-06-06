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
        if (commands.Count == 0 )
        {
            if (!CommandsEnded)
            {
                CommandsEnded = true;
                Debug.Log("All commands have ended.");

                onCommandsEnded?.Invoke(); // Invoke the event when commands end

            }
        }
        else
        {
            CommandsEnded = false;
        }
    }

}
