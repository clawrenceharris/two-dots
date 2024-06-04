
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static Type;
public enum ExecutionStage
{
    ClearAnchorsAtBottom,
    DropDots,
    ClearConnection
}
public class LevelManager : MonoBehaviour


{
    public static event Action OnLevelEnd;
    public static event Action OnLevelRestart;
    public LevelData Level { get; private set; }
    [SerializeField] private bool isTutorial;
    private ColorSchemeManager colorSchemeManager;
    public static int MoveCount { get; private set; } = 0;
    public bool IsHighscore { get; private set; }
    public int score;
    private bool didMove;
    [SerializeField] private ColorScheme[] colorSchemes;
    public bool IsTutorial
    {
        get
        {
            return isTutorial;
        }
    }
    private Board board;

    private static ColorScheme theme;

    public ColorScheme Theme
    {
        get
        {
            if (theme == null)
            {
                theme = FindObjectOfType<ColorScheme>();
            }
            return theme;
        }
    }

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        colorSchemeManager = new ColorSchemeManager(colorSchemes, 0);

    }

    private void Start()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void HandleDotConnected()
    {
        List<IHittable> dotsToHit = ConnectionManager.ToHit;



        foreach (IHittable hittable in dotsToHit)
        {
            CoroutineHandler.StartStaticCoroutine(hittable.Hit(HitType.Connection));
            if (hittable is IPreviewable previewable)
            {
                CoroutineHandler.StartStaticCoroutine(previewable.PreviewHit(HitType.Connection));
            }
        }

    }
    private void UnsubscribeFromEvents()
    {
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
        ConnectionManager.onDotConnected -= OnDotConnected;
        ConnectionManager.onDotDisconnected -= OnDotDisconnected;
        Command.onCommandExecuted -= OnCommandExecuted;
        CommandInvoker.onCommandsEnded -= OnCommnadsEnded;
        Board.onDotsDropped -= OnDotsDropped;
    }

    private void SubscribeToEvents()
    {
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        Command.onCommandExecuted += OnCommandExecuted;
        CommandInvoker.onCommandsEnded += OnCommnadsEnded;
        Board.onDotsDropped += OnDotsDropped;
    }

    private void OnDotDisconnected(ConnectableDot dot)
    {
        HandleDotConnected();
    }

    private void OnDotConnected(ConnectableDot dot)
    {
        HandleDotConnected();
    }

    public static void DestroyGO(GameObject go)
    {
        if (go != null && go.activeInHierarchy)
        {
            Destroy(go);
        }
    }
    private void OnCommandExecuted(Command command)
    {
        switch (command.CommandType)
        {
            case CommandType.Board:
                CommandInvoker.Instance.Enqueue(new HitCommand());
                break;
            case CommandType.Hit:
                CommandInvoker.Instance.Enqueue(new ClearCommand());

                break;
            case CommandType.Clear:
                CommandInvoker.Instance.Enqueue(new HitCommand());
                CommandInvoker.Instance.Enqueue(new BoardCommand());
                CommandInvoker.Instance.Enqueue(new ExplosionCommand());

                break;
        }
    }
    private void OnDotsDropped()
    {
        DoCommand(new BoardCommand());
    }

    private void OnConnectionEnded(LinkedList<ConnectableDot> dots)
    {
        didMove = true;
        DoCommand(new HitCommand());


    }

    private void OnCommnadsEnded()
    {
        if (didMove)
        {
            CommandInvoker.Instance.Enqueue(new MoveClockDotsCommand());
            CommandInvoker.Instance.Enqueue(new MoveBeetleDotsCommand());
            didMove = false;

        }
    }

    private void DoCommand(Command command)
    {
        CommandInvoker.Instance.Enqueue(command);
        CommandInvoker.Instance.ExecuteNextCommand();

    }

   
    public void StartLevel(int levelNum)
    {
        new CommandInvoker(board);
        new ConnectionManager(board);
        new DotController(board);
        Level = JSONLevelLoader.ReadJsonFile(levelNum);
        LinePool.Instance.FillPool(Level.width * Level.height);

        board.Init(Level);
        
        colorSchemeManager.SetColorScheme(Level.levelNum - 1);
        DoCommand(new BoardCommand());
        
    }



    public void LeaveLevel()
    {
        OnLevelEnd?.Invoke();

    }


    public void StartNextLevel()
    {
        OnLevelEnd?.Invoke();
        StartLevel(Level.levelNum + 1);

    }


    public void Restart()
    {
        OnLevelRestart?.Invoke();
        StartLevel(Level.levelNum);

    }



}
