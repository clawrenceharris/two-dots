
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
    public static bool DidMove { get; private set; }
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
        StartCoroutine(ExecuteCommands());

    }

    private IEnumerator ExecuteCommands()
    {
        yield return new WaitForSeconds(0.8f);

        yield return CommandInvoker.Instance.ExecuteCommands();
    }


    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;

        CommandInvoker.onCommandsEnded -= OnCommnadsEnded;

    }

    private void SubscribeToEvents()
    {

        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        CommandInvoker.onCommandsEnded += OnCommnadsEnded;

    }

    private void OnCommnadsEnded()
    {
        DidMove = false;
    }

    

    

   
    private void OnConnectionEnded(LinkedList<ConnectableDot> dots)
    {

        DidMove = true;
        StartCoroutine(CommandInvoker.Instance.ExecuteCommands());

        

    }

    

    public void StartLevel(int levelNum)
    {
        new CommandInvoker(board);
        new ConnectionManager(board);
        new DotsGameObjectController(board);
        new PreviewManager(board);

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
