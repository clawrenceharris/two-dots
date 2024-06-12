using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByExplosionRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        return Type.IsExplosion(hittable.HitType);
    }

}
