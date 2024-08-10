using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemExplosionRule : IExplosionRule
{
    public List<IHittable> Validate(IExplodable explodable, Board board)
    {
        if(explodable is not IBoardElement b){
            return new();
        }
        List<IHittable> toHit = new();
        //add all hittables that are in the same column 
        for(int row = 0; row < board.Height;  row++){
            List<IHittable> hittables = board.GetDotsGameObjectAt<IHittable>(explodable.Column, row);
            toHit.AddRange(hittables);
        }

        //add all hittables that are in the same row 
        for(int col = 0; col < board.Width;  col++){
            List<IHittable> hittables = board.GetDotsGameObjectAt<IHittable>(col, explodable.Row);
            toHit.AddRange(hittables);
        }
        return toHit;
    }
}
