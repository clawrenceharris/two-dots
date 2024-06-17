using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByNeighborsRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        List<Dot> neighbors = board.GetNeighbors<Dot>(hittable.Column, hittable.Row);

        foreach (Dot neighbor in neighbors)
        {
            //if the neighbor is not a connectable dot or it is not to be hit by the connection
            if (neighbor is not ConnectableDot connectableDot)
            {

                continue;

            }
            if (!ConnectionManager.ToHit.Contains(connectableDot))

                continue;


            AdjacentPositionRule adjacenyRule = new();
            if (adjacenyRule.Validate(hittable, connectableDot))
            {
                return true;
            }


        }
        
        return false;
    }
}
