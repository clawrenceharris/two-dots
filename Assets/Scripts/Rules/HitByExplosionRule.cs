using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitByExplosionRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        Debug.Log(hittable.HitType);
        return Type.IsExplosion(hittable.HitType);
    }

}
