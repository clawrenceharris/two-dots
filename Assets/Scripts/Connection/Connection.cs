using System.Collections.Generic;
using UnityEngine;
using System;

public class Connection
{

    private readonly SquareManager squareManager;

    public DotColor Color { get; private set; }
    public LinkedList<ConnectableDot> ConnectedDots { get; private set; }
    public List<ConnectableDot> DisconnectedDots { get; private set; }

    public bool IsSquare { get; private set; }
    public static event Action<Square> onSquareMade;
    
    public Connection(ConnectableDot dot, SquareManager squareManager)
    {

        ConnectedDots = new();
        ConnectedDots.AddLast(dot);
        IsSquare = false;
        Color = dot is IColorable colorDot ? colorDot.Color : DotColor.Blank;
        this.squareManager = squareManager;
    }


    private bool IsBlankConnection()
    {
        foreach (ConnectableDot dot in ConnectedDots)
        {
            if (!dot.DotType.IsBlank())
            {
                return false;
            }
        }
        return true;
    }
    
    public void ConnectDot(ConnectableDot dot)
    {
        
        dot.Connect(dot);
        ConnectedDots.AddLast(dot);

        //change the connection's color to match the dot's color if it is not blank
        if(!dot.DotType.IsBlank())
            Color = dot.Color;

        


    }
    public void CheckForSquare()
    {
        IsSquare = squareManager.ProcessSquare();
        if (IsSquare)
        {
            onSquareMade?.Invoke(squareManager.Square);

        }
    }
    public void DisconnectDot(ConnectableDot dot)
    {
        
        //remove the last dot from the connections list
        ConnectedDots.Remove(ConnectedDots.Last);

        //dont disconnect the dot if it is still connected
        if (!ConnectedDots.Contains(dot))
        {
            dot.Disconnect();

        }

        //if the connection has no color dots in it after discconection, change the connection's dotType to blank type 
        if (IsBlankConnection())
        {
            Color = DotColor.Blank;
            
        }

        IsSquare = false;

    }

    public bool Any<T>() where T : ConnectableDot
    {
        
        foreach (ConnectableDot dot in ConnectedDots)
        {
            if (dot is T)
            {
                return true;
            }
        }
        
        

        return false;
    }

    public void EndConnection()
    {
        ConnectedDots.Clear();

        IsSquare = false;

    }

    public List<T> FindDotsOfType<T>()
    {
        List<T> dots = new();
        foreach(ConnectableDot dot in ConnectedDots)
        {
            if (dot is T t)
            {
                dots.Add(t);
            }
        }
        return dots;
    }
}
