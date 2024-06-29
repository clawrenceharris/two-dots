using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Type;


public class CommandInvoker
{

    public class PriorityQueue<T>
    {
        private readonly SortedDictionary<int, Queue<T>> priorityQueue;

        public PriorityQueue()
        {
            priorityQueue = new();
        }

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
            if (priorityQueue.Count == 0) throw new InvalidOperationException("The queue is empty.");

            foreach (var key in priorityQueue.Keys)
            {
                var queue = priorityQueue[key];
                if (queue.Count > 0)
                {
                    var item = queue.Dequeue();
                    if (queue.Count == 0)
                    {
                        priorityQueue.Remove(key);
                    }
                    return item;
                }
            }

            throw new InvalidOperationException("The queue is empty.");
        }

       
        public bool IsEmpty => priorityQueue.Count == 0;

        public int Count
        {
            get { return priorityQueue.Values.Sum(queue => queue.Count); }
        }
    }
    
    //private readonly PriorityQueue<ICommand> commands = new(CommandComparer.Compare);
    private readonly Board board;
    private readonly Queue<ICommand> commands = new();

    public static CommandInvoker Instance;
    public static int commandCount;
    public static bool CommandsEnded { get; private set; } = true;
    private Coroutine checkCommandsEndedCoroutine;
    public static event Action onCommandsEnded;
    private bool isExecuting;
    public CommandInvoker(Board board)
    {
        Instance = this;
        Command.onCommandExecuted += OnCommandExecuted;
        this.board = board;
    }

    public void Enqueue(ICommand command)
    {
        CommandsEnded = false;
        commands.Enqueue(command);

        // Restart the check coroutine when a new command is executed
        RestartCheckCommandsEndedCoroutine();

    }

    public void ExecuteNextCommand()
    {
        if (commands.Count != 0)
        {
            ICommand command = commands.Dequeue();
            CoroutineHandler.StartStaticCoroutine(ExecuteCommandCo(command));
        }
        else
        {
            isExecuting = false;

        }
    }

    private IEnumerator ExecuteCommandCo(ICommand command)
    {
        isExecuting = true;
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
        yield return new WaitForSeconds(0.5f); 

        if (commands.Count == 0  && !CommandsEnded && !isExecuting)
        {
            
                CommandsEnded = true;
                Debug.Log(commandCount+ " All commands have ended.");

                onCommandsEnded?.Invoke();
        }
        
    }

}
