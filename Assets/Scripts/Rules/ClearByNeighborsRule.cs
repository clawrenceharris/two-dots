using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByNeighborsRule : IHitRule
{
    public bool Validate(IHittable hittable,Board board)
    {
        List<Dot> dots = board.GetDotNeighbors(hittable.Column, hittable.Row);
        Debug.Log("COUNT: " + ConnectionManager.ConnectedDots.Count);
        foreach(Dot dot in dots)
        {
            
            if (dot is not ConnectableDot connectableDot || !ConnectionManager.ConnectedDots.Contains(connectableDot))
            {
                continue;
            }
            if(dot.Column == hittable.Column -1)
            {
                return true;
            }
            if(dot.Column == hittable.Column + 1)
            {
                return true;
            }
            if(dot.Row == hittable.Row + 1)
            {
                return true;
            }
            if(dot.Row == hittable.Row - 1)
            {
                return true;
            }
        }
        return false;
    }
}
