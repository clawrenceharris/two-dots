using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  SquareGemExplosionRule : IExplosionRule
{
    public List<IHittable> Validate(IExplodable explodable, Board board)
    {
       
        List<IHittable> toHit = new();

        toHit.AddRange(board.FindDotsInRow(explodable.Row));        
        toHit.AddRange(board.FindDotsInColumn(explodable.Column));
        toHit.AddRange(board.FindBoardMechanicTilesInColumn<IHittable>(explodable.Column));        
        toHit.AddRange(board.FindBoardMechanicTilesInRow<IHittable>(explodable.Row));        
        return toHit;
    }

}
