using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitByConnectionRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        return ConnectionManager.ConnectedDots.Contains(hittable);
    }
}
