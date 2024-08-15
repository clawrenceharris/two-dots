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

        CommandInvoker.onCommandBatchCompleted += OnCommandBatchCompleted;
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
    }

    private void OnCommandBatchCompleted()
    {
        if(Square == null){
            return;
        }
        Square.ToHit.Clear();
        Square.DotsInSquare.Clear();
    }


   

    public bool ProcessSquare()
    {
        bool isSquare = CheckForSquare();
        //check if there is a square
        if (isSquare)
        {

            Square = DecideSquare();

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
    
    private Square DecideSquare(){
        if(ConnectionManager.Connection.Color.IsBlank()){
            return new BlankSquare(board);
        }
        else{
            return new Square(board);
        }
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




