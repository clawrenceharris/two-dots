using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;

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
        foreach (Dot dot in ConnectedDots)
        {
            if (dot is not IBlankDot)
            {
                return false;
            }
        }
        return true;
    }
    
    public void ConnectDot(ConnectableDot dot)
    {
        
        dot.Connect();
        ConnectedDots.AddLast(dot);

        //if we connected to a dot that is a color dot (not a blank dot), change the connection's color to that color 
        if(dot is IColorable colorDot)
            Color = colorDot.Color;

        IsSquare = squareManager.ProcessSquare();
        if (IsSquare)
        {
            onSquareMade?.Invoke(squareManager.Square);

        }

    }

   

    public void DisconnectDot(ConnectableDot dot)
    {
        dot.Disconnect();
        //remove the last dot from the connections list
        ConnectedDots.Remove(dot);
      
        //if the connection has no color dots in it after discconection, change the connection's dotType to blank type 
        if (IsBlankConnection())
        {
            Color = DotColor.Blank;
            
        }

        IsSquare = false;

    }

    
    public void ResetConnection()
    {
        ConnectedDots.Clear();
        IsSquare = false;
    }

   






}
