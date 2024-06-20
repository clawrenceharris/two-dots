using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionHitRule : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        return true;
    }
}

