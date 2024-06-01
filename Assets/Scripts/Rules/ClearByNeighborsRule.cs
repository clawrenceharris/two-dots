using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByNeighborsRule : IHitRule
{
    public bool Validate(IHittable hittable,Board board)
    {
        List<Dot> neighbors = board.GetDotNeighbors(hittable.Column, hittable.Row);

        foreach(Dot neighbor in neighbors)
        {
            //if the neighbor is not a connectable dot or it is not to be hit by the connection
            if (neighbor is not ConnectableDot connectableDot)
            {
                continue;
            }

            if (!ConnectionManager.ToHit.Contains(connectableDot))
            {
                continue;
            }

            //if the neighbor is to the left of the target hittable
            if(neighbor.Column == hittable.Column -1)
            {
                return true;
            }

            //if the neighbor is to the right of the target hittable
            if (neighbor.Column == hittable.Column + 1)
            {
                return true;
            }

            //if the neighbor is above the target hittable
            if (neighbor.Row == hittable.Row + 1)
            {
                return true;
            }

            //if the neighbor is below the target hittable
            if (neighbor.Row == hittable.Row - 1)
            {
                return true;
            }
        }
        return false;
    }
}
