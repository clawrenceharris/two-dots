using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class ConnectionManager
{

    public static event Action<ConnectableDot> onDotDisconnected;

    public static event Action<ConnectableDot> onDotConnected;

    public static event Action<ConnectableDot> onDotSelected;
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
            if (IsSquare)
            {
                return squareManager.Square.ToHit;
            }
            return new(ConnectedDots);
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
        DotTouchIO.onSelectionEnded += HandleSelectionEnded;
        DotTouchIO.onDotConnected += OnDotConnected;
        Command.onCommandExecuted += OnCommandExecuted;

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
            onDotDisconnected?.Invoke(dotToDisconnect);

        }

        // if the connection is valid and it is not currently a square
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

            ConnectableDot last = ConnectedDots.Last.Value;
            Connection.DisconnectDot(last);

            onDotDisconnected?.Invoke(last);
            return;
        }
        else
        {
            onConnectionEnded?.Invoke(ConnectedDots);
        }
    }
    

    private void OnCommandExecuted(Command command)
    {
        if(command is HitCommand)
        {
            Connection.EndConnection();
        }
    }

   
}

