using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class ConnectionManager
{

    public static event Action<ConnectionArgs> onDotDisconnected;

    public static event Action<ConnectionArgs> onDotConnected;

    public static event Action<ConnectionArgs> onDotSelected;
    public static event Action onSquare;
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


    public static List<IHittable> ToHit {get
        {
            List<IHittable> toHit = new(ConnectedDots);
            if (IsSquare)
            {
                toHit.AddRange(squareManager.Square.ToHit);
                return toHit;
            }
            return toHit;
        }
    }

    public static List<T> GetElementsToHit<T>()
    {
        List<T> toHit = new();

 
        ToHit.ForEach((hittable) => {
            if (hittable is T t)
                toHit.Add(t);
        });
        return toHit;
    }

    public static List<IHittable> ToHitBySquare
    {

        get
        {
            if (IsSquare)
            {

                return squareManager.Square.ToHit;
            }
            return new();

        }
    }






    public ConnectionManager(Board board)
    {
        
        squareManager = new SquareManager(board);
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        DotTouchIO.onDotSelected += OnDotSelected;
        DotTouchIO.onSelectionEnded += OnConnectionEnded;
        DotTouchIO.onDotConnected += OnDotConnected;
        CommandInvoker.onCommandBatchCompleted += OnCommandBatchCompleted;

    }





    private bool IsDotDisconnected(ConnectableDot dot)
    {

        // If there are less than 2 dots in the connection, it's not disconnected
        if (ConnectedDots.Count < 2)
            return false;

        // Get the second-to-last node
        var secondToLastNode = ConnectedDots.Last.Previous;
        //If the dot is the second-to-last dot it is disconnected
        return secondToLastNode.Value == dot;
    

    }

    private bool IsValidConnection(ConnectableDot dot)
    {
        ConnectionRule connectionRule = new();
        return connectionRule.Validate(ConnectedDots.Last.Value, dot);
    }

    public static void SelectAndConnectDot(ConnectionArgs args)
    {
        
        if(Connection == null)
        {
            Connection = new(args.Dot, squareManager);
            args.Dot.Select();
            
        }
        else
        {
            Connection.ConnectDot(args.Dot);
            onDotConnected?.Invoke(args);
        }
    }
    public static void ConnectDot(ConnectionArgs args)
    {
        
        if(Connection == null)
        {
            
            Connection = new(args.Dot, squareManager);
            
        }
        else
        {
            Connection.ConnectDot(args.Dot);
            onDotConnected?.Invoke(args);
        }
    }
    private void OnDotConnected(ConnectableDot dot)
    {
        
        LinkedListNode<ConnectableDot> lastDot = ConnectedDots.Last;
        if (ConnectedDots.Count == 0 || lastDot.Value == dot)
        {
            return;
        }

        if (IsDotDisconnected(dot))
        {

            ConnectableDot dotToDisconnect = lastDot.Value;
            Connection.DisconnectDot(dotToDisconnect);
            onDotDisconnected?.Invoke(new ConnectionArgs(dot));

        }

        // if the connection is valid and it is not currently a square
        else if (IsValidConnection(dot) && !IsSquare)
        {
            Connection.ConnectDot(dot);
            Connection.CheckForSquare();
            onDotConnected?.Invoke(new ConnectionArgs(dot));

        }
    }

    private void OnDotSelected(ConnectableDot dot)
    {

        Connection = new Connection(dot, squareManager);
        dot.Select();
        onDotSelected?.Invoke(new ConnectionArgs(dot));

    }

    private void OnConnectionEnded()
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
            onDotDisconnected?.Invoke(new ConnectionArgs(lastDot));
            return;
        }
        else
        {

            onConnectionEnded?.Invoke(ConnectedDots);
        }
        
        foreach (ConnectableDot dot in ConnectedDots)
        {
            dot.Disconnect();
        }

    }



    private void OnCommandBatchCompleted()
    {
        Connection?.EndConnection();

    }

    
}

public class ConnectionArgs{
    public ConnectableDot Dot;
    public bool PlaySound;
    public ConnectionArgs(ConnectableDot dot, bool playSound = true){
        Dot = dot; 
        PlaySound = playSound;
    }
}

