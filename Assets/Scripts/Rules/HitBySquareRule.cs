using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitBySquareRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        
         return ConnectionManager.ToHitBySquare.Contains(hittable);
    }
}
