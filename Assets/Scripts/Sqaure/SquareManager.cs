using System.Collections.Generic;
using System.Linq;
using static Type;
using System;
using Color = UnityEngine.Color;
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

            Square = GetSquare();

            //activate bombs if any
            Square.ActivateBombsInsideSquare();

            //select all the appropriate dots
            Square.SelectAndAddDotsForSquare();

        }
        return isSquare;

    }

    private void OnDotDisconnected(ConnectableDot dot)
    {
        //if no square was made we dont want to do anything 
        if (Square == null)
        {
            return;
        }
        Square.DeselectDotsForSquare();
        Square.DeactivateBombsInsideSquare();

        Square = null;

    }

    private Square GetSquare()
    {
        SquareType squareType = DecideSquareType();

        // Use the factory to create the appropriate square instance
        Square = SquareFactory.CreateSquare(squareType, board);
        return Square;
    }

    
    private SquareType DecideSquareType()
    {

        if (ConnectionManager.Connection.Color == DotColor.Blank)
        {
            return SquareType.BlankSqaure;
        }
        else if (ConnectionManager.Connection.Color != DotColor.Blank)
        {
            return SquareType.NoramlSqaure;
        }

        else
        {
            throw new Exception("Could not determine square type");
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
public class Square
{
    public List<IHittable> ToHit { get; private set; }
    protected Board board;
    public List<Dot> DotsInSquare { get; private set; } = new();
    public Square(Board board)
    {
        this.board = board;
        ToHit = new(ConnectionManager.ConnectedDots );
    }
    public void ActivateBombsInsideSquare()
    {

        DotsInSquare = FindDotsInsideSquare();
        foreach (Dot dot in DotsInSquare)
        {
            dot.gameObject.SetActive(false);
            board.CreateBomb(dot.Column, dot.Row);
        }
    }

    public void DeactivateBombsInsideSquare()
    {

        foreach (Dot dot in DotsInSquare)
        {
            dot.gameObject.SetActive(true);

            //get the bomb that is at this dot's position on the board
            //then destroy it
            Dot bomb = board.GetDotAt<Dot>(dot.Column, dot.Row);
            Object.Destroy(bomb.gameObject);

            //put the original dot back
            board.Put(dot, dot.Column, dot.Row);

        }

    }

    private List<Dot> FindDotsInsideSquare()
    {
        LinkedList<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots;
        HashSet<Dot> dotsInSquare = new();
        List<ConnectableDot> square = GetImmediateBombSquare();
        if (connectedDots.Count < 9)
        {
            return new();
        }
        for (int col = 0; col < board.Width; col++)
        {
            for (int row = 0; row < board.Height; row++)
            {
                Dot dot = board.GetDotAt<Dot>(col, row);
                //Make sure dot is not in the square and not on edge of board because there is no need to fill these
                if (dot && !square.Contains(dot) && !board.IsOnEdgeOfBoard(dot.Column, dot.Row))
                {
                    //Add each valid and unique dot found in the flood fill to the dots in square set and disregard dots within the current connection
                    dotsInSquare.UnionWith(BoundaryFill(dot, square).Where((dot) => !connectedDots.Contains(dot)));
                }
            }
        }

        return dotsInSquare.ToList();

    }

    // Method to get the connected dots that directly
    // surround the inner dots to turn into bombs 
    private List<ConnectableDot> GetImmediateBombSquare()
    {
        LinkedList<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots;

        LinkedListNode<ConnectableDot> tail = connectedDots.Last.Previous;
        List<ConnectableDot> square = new();

        //loops through the connected dots starting at the last
        //dot in the connection and stops when it reaches a duplicate of the last connected dot. 
        while (tail != null)
        {
            square.Add(tail.Value);
            if (tail.Value == connectedDots.Last.Value)
            {

                return square;
            }

            tail = tail.Previous;
        }
        square.Clear();
        return square;
    }

    private List<Dot> BoundaryFill(Dot startDot, List<ConnectableDot> square)
    {

        List<Dot> dotsInSquare = new();

        // Set to keep track of visited dots to avoid revisiting them
        HashSet<Dot> visitedDots = new();

        // Queue for breadth-first search traversal
        Queue<Dot> queue = new();

        // Enqueue the starting dot
        queue.Enqueue(startDot);

        // Perform breadth-first search
        while (queue.Count > 0)
        {
            Dot currentDot = queue.Dequeue();

            dotsInSquare.Add(currentDot);

            visitedDots.Add(currentDot);

            // Get neighbors of the current dot
            List<Dot> neighbors = board.GetDotNeighbors<Dot>(currentDot.Column, currentDot.Row, true);

            foreach (Dot neighbor in neighbors)
            {

                // Check if the neighbor is valid and has not been visited yet
                if (neighbor != null && !visitedDots.Contains(neighbor))
                {
                    // Check if the neighbor is on the edge of the board
                    if (board.IsOnEdgeOfBoard(neighbor.Column, neighbor.Row) &&
                        !square.Contains(neighbor))
                    {
                        // Clear the list of dots in the square and return an empty list
                        // indicating that no dots are inside the square
                        dotsInSquare.Clear();
                        return dotsInSquare;
                    }

                    // Check if the neighbor is inside the square (not in the connection)
                    else if (!square.Contains(neighbor))
                    {
                        // Add the neighbor to the queue for further exploration
                        queue.Enqueue(neighbor);
                    }

                    visitedDots.Add(neighbor);

                }
            }
        }

        return dotsInSquare;
    }



    // Returns whether the dot should be hit based on color, dot type, etc
    protected virtual bool ShouldBeHit(IHittable hittable)
    {

        if(hittable is not ConnectableDot dot)
        {
            return false;
        }
        return

            //the dot is not going to be a bomb
            !DotsInSquare.Contains(hittable) &&

            (dot.DotType.ShouldBeHitBySquare() ||
            ToHit.Contains(hittable) ||
            //the dot's color is the same as the connection color
            dot.Color == ConnectionManager.Connection.Color);

    }

    /// <summary>
    /// Selects the dots that would be hit by the square, and
    /// adds them to a list so we don't have to loop through all dots again to hit them
    /// </summary>
    public void SelectAndAddDotsForSquare()
    {
        for (int col = 0; col < board.Width; col++)
        {
            for (int row = 0; row < board.Height; row++)
            {
                Dot dot = board.GetDotAt<Dot>(col, row);

                if (dot is ConnectableDot connectableDot && ShouldBeHit(connectableDot))
                {
                    ToHit.Add(connectableDot);
                    connectableDot.Select();
                }

            }
        }
    }


/// <summary>
/// Deselects the dots that would of been hit by the square.
/// </summary>
public void DeselectDotsForSquare()
{
    for (int col = 0; col < board.Width; col++)
    {
        for (int row = 0; row < board.Height; row++)
        {
            Dot dot = board.GetDotAt<Dot>(col, row);

            if (dot is ConnectableDot connectableDot && ShouldBeHit(dot))
            {
                connectableDot.Deselect();
            }

        }
    }
}



    

}

public class BlankSquare : Square
{
    public BlankSquare(Board board) : base(board) { }



    

}


