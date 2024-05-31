
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
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        Command.onCommandExecuted += OnCommandExecuted;

        Board.onDotsDropped += OnDotsDropped;
    }

    private void OnDotDisconnected(ConnectableDot dot)
    {
        DoCommand(new ConnectDotsCommand());

    }

    private void OnDotConnected(ConnectableDot dot)
    {
        DoCommand(new ConnectDotsCommand());        
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
                CommandInvoker.Instance.Enqueue(new HitDotsCommand());
                CommandInvoker.Instance.Enqueue(new HitTilesCommand());
                CommandInvoker.Instance.Enqueue(new ExplosionCommand());
                break;
            case CommandType.HitDots:
            case CommandType.HitTiles:
                CommandInvoker.Instance.Enqueue(new ClearCommand());
                break;
            case CommandType.Clear:
                CommandInvoker.Instance.Enqueue(new HitDotsCommand());
                CommandInvoker.Instance.Enqueue(new HitTilesCommand());
                if (didMove)
                    CommandInvoker.Instance.Enqueue(new MoveClockDotsCommand());

                CommandInvoker.Instance.Enqueue(new BoardCommand());
                didMove = false;
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
        DoCommand(new HitDotsCommand());
        DoCommand(new HitTilesCommand());


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
