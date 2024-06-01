using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class HitByNeighborRule : IHitRule
{


   

    public bool Validate(IHittable hittable,Board board)
    {
        List<Dot> dots = board.GetDotNeighbors(hittable.Column, hittable.Row);
        foreach (Dot dot in dots)
        {
            if(dot is not ConnectableDot connectableDot)
            {
                continue;
            }
            if (!ConnectionManager.ToHit.Contains(connectableDot))
            {
                continue;
            }

            if(hittable is IDirectional directional)
            {
                if (directional.Column + directional.DirectionX == dot.Column &&
                directional.Row + directional.DirectionY == dot.Row)
                {   
                    return true;
                }
                
            }
            
            
            

        }
        

        return false;
    }
}
