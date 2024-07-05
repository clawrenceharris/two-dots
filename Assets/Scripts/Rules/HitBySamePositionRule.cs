using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBySamePositionRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        if (hittable is not IBoardElement boardElement)
        {
            return false;
        }
        List<IBoardElement> elementsToHit = ConnectionManager.GetElementsToHit<IBoardElement>();
        foreach (IBoardElement toHit in elementsToHit)
        {
            Debug.Log(toHit.Column + "==" + boardElement.Column + "&&" + toHit.Row + "==" + boardElement.Row);
            if (toHit.Column == boardElement.Column && toHit.Row == boardElement.Row)
            {
                return true;
            }

        }
        return false;
    }

   
}
