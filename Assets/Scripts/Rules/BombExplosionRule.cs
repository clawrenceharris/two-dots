using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BombExplosionRule : IExplosionRule
{
    public List<IHittable> Validate(IExplodable explodable, Board board)
    {
        if (explodable is not IBoardElement boardElement)
        {
            return new();
        }
        List<IHittable> toHit = new();
        List<IHittable> neighbors = board.GetNeighbors<IHittable>(explodable.Column, explodable.Row, true);
        toHit.Add(explodable);

        foreach (IHittable neighbor in neighbors)
        {
            if(neighbor is IHittable hittable)
            {
                toHit.Add(hittable);
            }
        }

        return  toHit.OrderBy(dot => boardElement.Column).ThenByDescending(dot => boardElement.Row).ToList();
        

    }

    
}
