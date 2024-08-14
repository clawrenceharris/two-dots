using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  GemExplosionRule : IExplosionRule
{
    public List<IHittable> Validate(IExplodable explodable, Board board)
    {
       
        List<IHittable> toHit = new();
        //add all hittables that are in the same column 
        for (int row = 0; row < board.Height; row++)
        {
            IHittable hittable = board.GetDotAt<IHittable>(explodable.Column, row);
            if (hittable != null && !toHit.Contains(hittable))
            {
                toHit.Add(hittable);
            }
        }

        // Add all hittables that are in the same row
        for (int col = 0; col < board.Width; col++)
        {
            IHittable hittable = board.GetDotAt<IHittable>(col, explodable.Row);
            if (hittable != null && !toHit.Contains(hittable))
            {
                toHit.Add(hittable);
            }
            
        }
        return toHit;
    }

}
