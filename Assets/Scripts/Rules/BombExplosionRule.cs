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

        List<IHittable> dots = board.GetDotNeighbors<IHittable>(explodable.Column, explodable.Row, true);
        List<IHittable> tiles = board.GetBoardMechanicTileNeighbors<IHittable>(explodable.Column, explodable.Row, true);
        List<IHittable> toHit = dots.Concat(tiles).ToList();

        //add this bomb in the list so it will be hit and cleared along with the other dots and tiles
        toHit.Add(explodable);



        toHit.Sort((a, b) =>
        {
            if (a == null && b == null) return 0; // Both are null, consider them equal
            if (a == null) return -1; // Null is considered less than non-null
            if (b == null) return 1; // Non-null is considered greater than null

            return Compare((DotsGameObject)a, (DotsGameObject)b); 
        });

        return toHit;
    }

    public int Compare(DotsGameObject a, DotsGameObject b)
    {
        int compareX = a.Column.CompareTo(b.Column);
        return compareX == 0 ? a.Row.CompareTo(b.Row) : compareX;
    }


}
