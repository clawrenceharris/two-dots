using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusDotHitRule : IHitRule
{
    /// <summary>
    /// Checks if a given lotus dot should be hit.
    /// A lotus dot can be hit if any of its neighbors is the same color or is a Moonstone dot
    /// </summary>
    /// <param name="hittable">The Lotus dot</param>

    /// <param name="board">The game board</param>
    /// <returns>Whether the lotus dot can be connected to</returns>
    public bool Validate(IHittable hittable, Board board)
    {
        if (hittable is not LotusDot dot)
        {
            return false;
        }
        if(IsInConnection(dot)){
            return true;
        }

        if(HasValidNeighbor(dot, board)){
            return true;
        }

        return false;
        
    }

    private bool HasValidNeighbor(LotusDot dot, Board board){
    
    
        List<ConnectableDot> neighbors = board.GetDotNeighbors<ConnectableDot>(dot.Column, dot.Row, false);
        
        foreach (ConnectableDot neighbor in neighbors)
        {
            if (!neighbor)
            {
                continue;
            }
            if (neighbor.Color == dot.Color)
            {
                return true;
            }
            if (neighbor.DotType.IsMoonstoneDot())
            {
                return true;
            }
            
        }
        return false;

    }
    private bool IsInConnection(LotusDot dot){
        return ConnectionManager.ToHit.Contains(dot);
    }
}