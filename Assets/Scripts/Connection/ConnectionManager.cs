using UnityEngine;
using System;
using System.Collections.Generic;
using static Type;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;

public class ConnectionManager
{

    public static event Action<ConnectableDot> onDotDisconnected;

    public static event Action<ConnectableDot> onDotConnected;

    public static event Action<ConnectableDot> onDotSelected;
    public static event Action onSquare;
    private readonly List<IConnectionRule> connectionRules;
    private static SquareManager squareManager;
    public static event Action<LinkedList<ConnectableDot>> onConnectionEnded;
    public static Connection Connection { get; private set; }


    public static LinkedList<ConnectableDot> ConnectedDots
    {
        get
        {
            if (Connection != null)
                return Connection.ConnectedDots;
            return new();
        }
    }

    public static bool IsSquare
    {
        get
        {
            if(Connection != null)
            {
                return Connection.IsSquare;
            }
            return false;
            
        }
    }
    public static List<IHittable> ToHit {

        get
        {
            if(IsSquare)
            {
                return squareManager.Square.ToHit;
            }
            return new(ConnectedDots);

        }
    }

    public static List<IHittable> ToHitBySquare
    {

        get
        {
            if (IsSquare)
            {
                Debug.Log("IS SQUARE");

                return squareManager.Square.ToHit;
            }
            return new();

        }
    }






    public ConnectionManager(Board board)
    {
        
        squareManager = new SquareManager(board);
        connectionRules = new() { new ConnectByPositionRule(), new ConnectByColorRule() };
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        DotTouchIO.onDotSelected += OnDotSelected;
        DotTouchIO.onSelectionEnded += HandleSelectionEnded;
        DotTouchIO.onDotConnected += OnDotConnected;
        CommandInvoker.onCommandsEnded += OnCommandsExecuted;
    }





    private bool IsDotDisconnected(ConnectableDot dot)
    {

            // If there are less than 2 dots in the connection, it's not disconnected
            if (ConnectedDots.Count < 2)
                return false;

            // Get the second-to-last node
            var secondToLastNode = ConnectedDots.Last.Previous;
            // Check if the dot is the second-to-last dot
            return secondToLastNode.Value == dot;
        

    }

    private bool IsValidConnection(ConnectableDot dot)
    {
        foreach(IConnectionRule rule in connectionRules)
        {
            if(!rule.Validate(dot))
            {
                return false;
            }
        }
        return true;
    }

    private void OnDotConnected(ConnectableDot dot)
    {
        LinkedListNode<ConnectableDot> lastDot = ConnectedDots.Last;
        if (ConnectedDots.Count == 0 || lastDot.Value == dot)
        {
            return;
        }


        // if the dot we drag over is the second to last dot in the connections list
        if (IsDotDisconnected(dot))
        {

            //disconnect it
            ConnectableDot dotToDisconnect = lastDot.Value;
            Connection.DisconnectDot(dotToDisconnect);
            //ConnectedDots = new(Connection.Dots);

            onDotDisconnected?.Invoke(dotToDisconnect);

        }

        // otherwise if the connection between the last dot and the dot we drag over is valid
        else if (IsValidConnection(dot) && !IsSquare)
        {
            Connection.ConnectDot(dot);          
            onDotConnected?.Invoke(dot);

        }
    }

    private void OnDotSelected(ConnectableDot dot)
    {

        Connection = new Connection(dot, squareManager);
        dot.Select();
        onDotSelected?.Invoke(dot);

    }




    private void HandleSelectionEnded()
    {
        
        if (IsSquare)
        {
            onSquare?.Invoke();
        }

        //if theres only one dot in the connection, deselect it
        if (ConnectedDots.Count == 1)
        {

            ConnectableDot lastDot = ConnectedDots.Last.Value;
            Connection.DisconnectDot(lastDot);

            onDotDisconnected?.Invoke(lastDot);
            return;
        }
        else
        {
            onConnectionEnded?.Invoke(ConnectedDots);
        }

    }

    private void OnCommandsExecuted()
    {          
        Connection?.ResetConnection();    
    }

}

