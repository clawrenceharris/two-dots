using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



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
    private readonly PriorityQueue<ICommand> commands = new();

    public static CommandInvoker Instance;
    public static int commandCount;
    public static bool CommandsEnded { get; private set; } = true;
    public static event Action onCommandsEnded;
    public static event Action onCommandBatchCompleted;

    public bool IsExecuting { get; private set; }
    public CommandInvoker(Board board)
    {
        Instance = this;
        Command.onCommandExecuted += OnCommandExecuted;
        this.board = board;
    }

   
    public Command GetNextCommand(CommandType type)
    {
        switch (type)
        {
            case CommandType.Board:
                return new HitCommand();
            case CommandType.Hit:
                return new ClearCommand();
            case CommandType.Clear:
                return new BoardCommand();
            default: throw new ArgumentException();
        }
    }

    public void Enqueue(ICommand command)
    {
        CommandsEnded = false;
        commands.Enqueue(command,(int) command.CommandType);

        

    }

    


    public IEnumerator ExecuteNextCommand(Action<bool> onCommandFinished = null)
    {
        while (commands.Count > 0)
        {
            ICommand command = commands.Dequeue();
            yield return command.Execute(board);
            onCommandFinished?.Invoke(command.DidExecute);
        }

       
    }
    
   

    private void OnCommandExecuted(Command command)
    {
        commandCount++;
    }

    
    public IEnumerator ExecuteLateCommands()
    {
        bool keepGoing = false;
        Enqueue(new ExplosionCommand());
        yield return ExecuteNextCommand((didExecute) =>
        {
            if (didExecute)
            {
                keepGoing = true;
            }
        });
        if (keepGoing)
        {
            yield return ExecuteCommands();
        }



    }

    public IEnumerator ExecuteCommands()
    {
        bool keepGoing = false;
        do
        {
            IsExecuting = true;

            keepGoing = false;

            Enqueue(new SinkAnchorDotsCommand());
            Enqueue(new ConnectLotusDotsCommand());

            Enqueue(new HitCommand());
            Enqueue(new MoveClockDotsCommand());

            Enqueue(new ClearCommand());
            Enqueue(new BoardCommand());


            yield return ExecuteNextCommand((didExecute) =>
            {
                if (didExecute)
                {
                    keepGoing = true;
                }
            });

            onCommandBatchCompleted?.Invoke();
        } while (keepGoing);
        IsExecuting = false;

        yield return ExecuteLateCommands();

        //all commands have finished
        
       
        if (LevelManager.DidMove) {

            Debug.Log(commandCount + " All commands have ended.");

            onCommandsEnded?.Invoke();
            Enqueue(new MoveBeetleDotsCommand());
            Enqueue(new MoveMonsterDotsCommand());
            CoroutineHandler.StartStaticCoroutine(ExecuteCommands());

        }
        //execute commands



    }

   


    
}
