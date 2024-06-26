using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Type;
public static class CommandComparer
{
    public static int Compare(Command a, Command b)
    {
       

        // If none of the specific rules apply, use natural order
        return a.CommandType.CompareTo(b.CommandType);
    }
}


public class CommandInvoker
{

    public class PriorityQueue<T>
    {
        private readonly SortedDictionary<int, Queue<T>> priorityQueue;
        private readonly Func<T, T, int> comparer;

        public PriorityQueue(Func<T, T, int> comparer)
        {
            priorityQueue = new SortedDictionary<int, Queue<T>>();
            this.comparer = comparer;
        }

        public void Enqueue(T item)
        {
            int priority = GetPriority(item);

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

        private int GetPriority(T item)
        {
            // Using the comparer to determine priority
            foreach (var key in priorityQueue.Keys)
            {
                var sampleItem = priorityQueue[key].Peek();
                if (comparer(item, sampleItem) < 0)
                {
                    return key - 1;
                }
                if (comparer(item, sampleItem) == 0)
                {
                    return key;
                }
            }
            return priorityQueue.Count == 0 ? 0 : priorityQueue.Keys.Max() + 1;
        }

        public bool IsEmpty => priorityQueue.Count == 0;

        public int Count
        {
            get { return priorityQueue.Values.Sum(queue => queue.Count); }
        }
    }
    
    private readonly PriorityQueue<Command> commands = new(CommandComparer.Compare);
    private readonly Board board;
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

    public void Enqueue(Command command)
    {
        CommandsEnded = false;
        commands.Enqueue(command);

        // Restart the check coroutine when a new command is executed
        RestartCheckCommandsEndedCoroutine();

    }

    public void ExecuteNextCommand()
    {
        if (!commands.IsEmpty)
        {
            Command command = commands.Dequeue();
            CoroutineHandler.StartStaticCoroutine(ExecuteCommandCo(command));
        }
        else
        {
            isExecuting = false;

        }
    }

    private IEnumerator ExecuteCommandCo(Command command)
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
       
        yield return new WaitForSeconds(1f); 

        if (commands.IsEmpty  && !CommandsEnded && !isExecuting)
        {
            
                CommandsEnded = true;
                Debug.Log(commandCount+ " All commands have ended.");

                onCommandsEnded?.Invoke();
        }
        
    }

}
