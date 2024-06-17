using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionRule : IExplosionRule
{
    public List<IHittable> Validate(IExplodable explodable, Board board)
    {
        List<IHittable> toHit = new();
        List<IHittable> hittables = board.GetNeighbors<IHittable>(explodable.Column, explodable.Row, true);
        foreach(IHittable hittable in hittables)
        {
            if(hittable != null)
            {
                toHit.Add(hittable);
            }
        }

        return toHit;

    }

    
}
