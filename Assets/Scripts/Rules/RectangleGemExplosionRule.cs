using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleGemExplosionRule : IExplosionRule
{
    public List<IHittable> Validate(IExplodable explodable, Board board)
    {
        if(explodable is not RectangleGem gem){
            return new();
        }
        
        List<IHittable> toHit = new();
        //add all hittables that are in the same column 
        if(gem.DirectionX > 0){
            toHit.AddRange(board.FindDotsInRow(gem.Row));
            toHit.AddRange(board.FindBoardMechanicTilesInRow<IHittable>(gem.Row));

        }
        else if(gem.DirectionY > 0){
            toHit.AddRange(board.FindDotsInColumn(gem.Column));
            toHit.AddRange(board.FindBoardMechanicTilesInColumn<IHittable>(gem.Column));

        }
       
        return toHit;;
    }


}
