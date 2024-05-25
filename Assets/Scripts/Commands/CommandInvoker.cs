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

public class CommandInvoker : MonoBehaviour
{
    private readonly PriorityQueue<Command> commands = new();
    private Board board;
    public static CommandInvoker Instance;
    public static event Action onCommandsExecuted;
    public static int commandCount;
    public static bool IsExecuting { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Command.onCommandExecuted += OnCommandExecuted;
        board = FindObjectOfType<Board>();


    }



    public void Enqueue(Command command)
    {

       
        commands.Enqueue(command, (int)command.CommandType);
        
    }



    public void ExecuteNextCommand()
    {

        if (commands.Count > 0)
        {
            
            StartCoroutine(ExecuteCommandCo());
        }
        else
        {
            IsExecuting = false;
            onCommandsExecuted?.Invoke();

        }



    }

    private IEnumerator ExecuteCommandCo()
    {
        Command command = commands.Dequeue();

        IsExecuting = true; // Set flag to indicate that commands are executing
        yield return StartCoroutine(command.Execute(board)); // Execute the command
        ExecuteNextCommand();
    }

    private void OnCommandExecuted(Command command)
    {
        commandCount++;
        
        
    }
}
