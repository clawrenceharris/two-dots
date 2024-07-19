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
        if (hittable is not IColorable lotusDot || hittable is not IBoardElement b)
        {
            return false;
        }

        List<ConnectableDot> neighbors = board.GetDotNeighbors<ConnectableDot>(b.Column, b.Row, false);

        foreach (ConnectableDot neighbor in neighbors)
        {
            if (!neighbor)
            {
                continue;
            }
            if (neighbor.Color == lotusDot.Color)
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
}