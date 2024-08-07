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

        Dot dot = board.GetDotAt<Dot>(b.Column, b.Row);
        if(dot != null){
            return dot.WasHit;
        }
            
        

        

        return false;
    }

   
}
