using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class HitByAdjacentColorRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        if(hittable is not IBoardElement b || hittable is not IColorable colorable)
        {
            return false;
        }
        List<IColorable> colorableNeighbors = board.GetDotNeighbors<IColorable>(b.Column, b.Row, false);

        foreach (IColorable neighbor in colorableNeighbors)
        {
            if (neighbor != null)
            {
                if (neighbor.Color == colorable.Color)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
