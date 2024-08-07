using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByNeighborsRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        if(hittable == null || hittable is not IBoardElement boardElement)
        {
            return false;
        }
        List<ConnectableDot> neighbors = board.GetDotNeighbors<ConnectableDot>(boardElement.Column, boardElement.Row, false);

        foreach (ConnectableDot neighbor in neighbors)
        {
            //if the neighbor is a connectable dot or it was hit
            if (ConnectionManager.ToHit.Contains(neighbor))
                return true;
            
            
        }
        
        return false;
    }
}
