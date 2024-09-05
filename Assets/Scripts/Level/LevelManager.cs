
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



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

    

    public void StartLevel(LevelData level)
    {
        new CommandInvoker(board);
        new ConnectionManager(board);
        Level = level;
        LinePool.Instance.FillPool(level.width * level.height);

        board.Init(level);
        
        colorSchemeManager.SetColorScheme(level.levelNum - 1);

    }



    public void LeaveLevel()
    {
        OnLevelEnd?.Invoke();

    }


    public void StartNextLevel()
    {
        OnLevelEnd?.Invoke();
        LevelData level = JSONLevelLoader.LoadLevelData(Level.levelNum + 1);
        StartLevel(level);

    }


    public void Restart()
    {
        OnLevelRestart?.Invoke();
        StartLevel(Level);

    }



}
