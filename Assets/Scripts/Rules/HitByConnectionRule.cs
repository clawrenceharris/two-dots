using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitByConnectionRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        return ConnectionManager.ToHit.Contains(hittable) && 
        ConnectionManager.ConnectedDots.Count > 1;
    }
}
