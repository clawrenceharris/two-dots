using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
public class Square
{
    // Set to keep track of visited dots to avoid revisiting them
    readonly HashSet<Dot> visitedDots = new();

    public List<IHittable> ToHit { get; private set; }
    protected Board board;
    public List<Dot> DotsInSquare { get; private set; } = new();
    public Square(Board board)
    {
        this.board = board;
        ToHit = new();
    }

   

    public void ActivateBombsInsideSquare()
    {

        DotsInSquare = FindDotsInsideSquare();
        foreach (Dot dot in DotsInSquare)
        {
            dot.gameObject.SetActive(false);
            board.SpawnBomb(dot.Column, dot.Row);
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
        List<ConnectableDot> square = GetBombSquare();
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

    /// <summary>
    /// Returns the connected dots that directly
    /// surround the inner dots to turn into bombs 
    /// </summary>
    /// <returns></returns>
    // 
    private List<ConnectableDot> GetBombSquare()
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

    /// <summary>
    /// Performs a boundary fill algorithm starting from a given dot, filling all connected dots that are inside the specified square area.
    /// </summary>
    /// <param name="startDot">The starting dot from which the boundary fill algorithm begins.</param>
    /// <param name="square">A list of connectable dots representing the boundaries of the square area to be filled.</param>
    /// <returns>A list of dots that are inside the square area. If a dot is on the edge of the board the list will be cleared and returned empty.</returns>
    /// <remarks>
    /// This method uses a breadth-first search approach to traverse and fill dots starting from the given <paramref name="startDot"/>.
    /// It maintains a queue to keep track of dots to be processed, and a hash set to track visited dots to avoid processing the same dot multiple times.
    /// If a dot is found on the edge of the board and not within the specified <paramref name="square"/>, the method will clear the result list and return an empty list, 
    /// indicating that no dots are inside the square or that the boundary was reached.
    /// </remarks>
    private List<Dot> BoundaryFill(Dot startDot, List<ConnectableDot> square)
    {

        List<Dot> dotsInSquare = new();

       
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
                bool isOnEdge = board.IsOnEdgeOfBoard(neighbor.Column, neighbor.Row);
                bool isInSquare = square.Contains(neighbor);
                
                if(visitedDots.Contains(neighbor)){
                    continue;
                }
                // If the neighbor is on the edge and not part of the square we know that it cant be inside the square
                if (isOnEdge && !isInSquare)
                {
                    //then return empty list
                    return new();
                }
                
                // If the neighbor is not part of the square connection, then add it to the queue for further exploration
                // otherwise continue checking the other neighbors of the current dot
                if (!isInSquare)
                {
                    
                    queue.Enqueue(neighbor);
                }
                
                    
                visitedDots.Add(neighbor);

            }
            
        }

        return dotsInSquare;
    }


    protected virtual bool ShouldHitDot(Dot dot){

        if(dot is not IColorable colorable){
            return false;
        }

        if(dot.DotType.ShouldBeHitBySquare() || 
            colorable.Color == ConnectionManager.Connection.Color){
            return true;
        }
        return false;
    }
    protected virtual bool ShouldHitTile(Tile tile){
        if(tile is not IColorable colorable){
            return false;
        }

        if(tile.TileType.ShouldBeHitBySquare() ||colorable.Color == ConnectionManager.Connection.Color){
            return true;
        }
        return false;
    }
    /// <summary>
    /// Returns whether the hittable object should be hit based on color, type, 
    /// and position
    /// </summary>
    /// <param name="hittable">The hittable object to check</param>
    /// <returns></returns>
    private  bool ShouldHit(IHittable hittable)
    {
        if(DotsInSquare.Contains(hittable) ){
            return false;
        }
        else if(hittable is Dot dot)
            return ShouldHitDot(dot);
        else if(hittable is Tile tile){
            return ShouldHitTile(tile);
        }
        return false;

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

                if (dot is IColorable colorDot && ShouldHit(dot))
                {
                    ToHit.Add(dot);
                    colorDot.Select();
                }

            }
        }
    }


    /// <summary>
    /// Deselects the dots that would of been hit by the square.
    /// </summary>
    public void DeselectDotsFromSquare()
    {
        foreach(IHittable hittable in ToHit){
            if (hittable is ConnectableDot connectableDot){
                if(!ConnectionManager.ConnectedDots.Contains(connectableDot))
                    connectableDot.Deselect();
            }
        }
    }   

}