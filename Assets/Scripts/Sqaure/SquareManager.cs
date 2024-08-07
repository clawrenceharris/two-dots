using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class SquareManager
{
    protected Board board;
    public Square Square { get; private set; }
   
    public SquareManager(Board board)
    {
        this.board = board;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;

        Command.onCommandExecuted += OnCommandExecuted;
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
    }

    

    private void OnCommandExecuted(Command command)
    {
        if (command is ExplodeCommand && Square != null)
            Square.DotsInSquare.Clear();
    }

    

    public bool ProcessSquare()
    {
        bool isSquare = CheckForSquare();
        //check if there is a square
        if (isSquare)
        {

            Square = new Square(board);

            //activate bombs if any
            Square.ActivateBombsInsideSquare();

            //select all the appropriate dots
            Square.SelectAndAddDotsForSquare();

        }
        return isSquare;

    }

    private void OnDotDisconnected(ConnectableDot dot)
    {
        if (Square == null)
        {
            return;
        }
        Square.DeselectDotsFromSquare();
        Square.DeactivateBombsInsideSquare();

        Square = null;

    }

    private void OnConnectionEnded(LinkedList<ConnectableDot> dots){
        if (Square == null)
        {
            return;
        }
        
        Square.DeselectDotsFromSquare();
    }
   
    
    public static bool CheckForSquare()
    {
        LinkedList<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots;

        //find duplicate dots in the connection
        var duplicates = connectedDots.GroupBy(obj => obj)
                        .Where(group => group.Count() > 1)
                        .SelectMany(group => group);

        //if there are exactly two of the same dot in the connection 
        if (duplicates.Count() == 2)
        {

            return true;
        }

        return false;

    }

}




