using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByNeighborsRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        if(hittable is not IBoardElement boardElement)
        {
            return false;
        }
        List<Dot> neighbors = board.GetDotNeighbors<Dot>(boardElement.Column, boardElement.Row);

        foreach (Dot neighbor in neighbors)
        {
            //if the neighbor is not a connectable dot or it is not to be hit by the connection
            if (neighbor is not ConnectableDot connectableDot)
            {

                continue;

            }
            if (!ConnectionManager.GetElementsToHit<ConnectableDot>().Contains(connectableDot))

                continue;


            AdjacentPositionRule adjacenyRule = new();
            if (adjacenyRule.Validate(boardElement, connectableDot))
            {
                return true;
            }


        }
        
        return false;
    }
}
