using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBySamePositionRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        if (hittable is not IBoardElement b)
        {
            return false;
        }

        List<Dot> dots = board.GetDots();
        foreach (Dot dot in dots)
        {
            //if the dot is at the same position as the target hittable
            if (dot.Column == b.Column && dot.Row == b.Row)
            {
                //return true if the hittable was just hit
                return dot.WasHit;
            }
        }

        

        return false;
    }

   
}
