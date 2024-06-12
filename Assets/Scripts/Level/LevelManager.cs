
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
    
    private void UnsubscribeFromEvents()
    {
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
        ConnectionManager.onDotConnected -= OnDotConnected;
        ConnectionManager.onDotDisconnected -= OnDotDisconnected;
        Command.onCommandExecuted -= OnCommandExecuted;
        CommandInvoker.onCommandsEnded -= OnCommnadsEnded;
    }

    private void SubscribeToEvents()
    {
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        Command.onCommandExecuted += OnCommandExecuted;
        CommandInvoker.onCommandsEnded += OnCommnadsEnded;
    }




    private void OnDotDisconnected(ConnectableDot dot)
    {
        if (dot is IPreviewable previewable)
        {
            StartCoroutine(previewable.PreviewHit(PreviewHitType.None));
        }
    }

    private void OnDotConnected(ConnectableDot dot)
    {
        List<IHittable> toHit = ConnectionManager.ToHit;

        foreach (IHittable hittable in toHit)
        {
            if (hittable is IPreviewable previewable)
            {
                StartCoroutine(previewable.PreviewHit(PreviewHitType.Connection));
            }
        }
    }

    private void OnCommandExecuted(Command command)
    {
        switch (command.CommandType)
        {

            case CommandType.Board:
                CommandInvoker.Instance.Enqueue(new HitCommand());
                CommandInvoker.Instance.Enqueue(new ExplosionCommand());
                break;
           
            case CommandType.Hit:
                CommandInvoker.Instance.Enqueue(new BoardCommand());
                break;
            case CommandType.Explosion:
                CommandInvoker.Instance.Enqueue(new HitCommand());
                break;

        }
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

            DoCommand(new MoveClockDotsCommand());
            DoCommand(new MoveBeetleDotsCommand());
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
