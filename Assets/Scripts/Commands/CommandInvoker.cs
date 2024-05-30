using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class PriorityQueue<T>
{
    private SortedDictionary<int, Queue<T>> priorityQueue = new SortedDictionary<int, Queue<T>>();

    public void Enqueue(T item, int priority)
    {
        if (!priorityQueue.ContainsKey(priority))
        {
            priorityQueue[priority] = new Queue<T>();
        }
        priorityQueue[priority].Enqueue(item);
    }

    public T Dequeue()
    {
        var item = priorityQueue.First().Value.Dequeue();
        if (priorityQueue.First().Value.Count == 0)
        {
            priorityQueue.Remove(priorityQueue.First().Key);
        }
        return item;
    }

    public int Count
    {
        get { return priorityQueue.Values.Sum(queue => queue.Count); }
    }
}


public class CommandInvoker
{
    private readonly Queue<Command> commands = new();
    private Board board;
    public static CommandInvoker Instance;
    public static event Action<Queue<Command>> onCommandsExecuted;
    public static int commandCount;
    private Queue<Command> tempCommands = new();
    public static bool IsExecuting { get; private set; }
    

    public CommandInvoker(Board board)
    {
        Instance = this;

        Command.onCommandExecuted += OnCommandExecuted;
        this.board = board;

    }


    public void ExecuteCommand(Command command)
    {
        CoroutineHandler.StartStaticCoroutine(ExecuteCommandCo(command));
    }


    public void Enqueue(Command command)
    {

       
        commands.Enqueue(command);
        tempCommands.Enqueue(command);
    }



    public void ExecuteNextCommand()
    {

        if (commands.Count > 0)
        {
            Command command = commands.Dequeue();

            CoroutineHandler.StartStaticCoroutine(ExecuteCommandCo(command));
        }
        else
        {
            IsExecuting = false;
            onCommandsExecuted?.Invoke(tempCommands);
            tempCommands.Clear();
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

   

}
