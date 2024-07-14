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

        List<IHittable> hittables = board.FindElementsOfType<IHittable>();
        foreach (IHittable h in hittables)
        {
            if (h is IBoardElement b)
                if (b.Column == boardElement.Column && b.Row == boardElement.Row)
                {

                    return h.HitType.IsConnection() || h.HitType.IsExplosion();
                }
        }

        

        return false;
    }

   
}
