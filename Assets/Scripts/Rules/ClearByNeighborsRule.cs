using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByNeighborsRule : IHitRule
{
    public bool Validate(IHittable hittable,Board board)
    {
        List<Dot> dots = board.GetDotNeighbors(hittable.Column, hittable.Row);

        foreach(Dot dot in dots)
        {
            if (dot is not ConnectableDot || !dots.Contains(dot))
            {
                continue;
            }
            if(dot.Column - 1 == hittable.Column)
            {
                return true;
            }
            if(dot.Column + 1 == hittable.Column)
            {
                return true;
            }
            if(dot.Row + 1 == hittable.Row)
            {
                return true;
            }
            if(dot.Row - 1 == hittable.Row)
            {
                return true;
            }
        }
        return false;
    }
}
