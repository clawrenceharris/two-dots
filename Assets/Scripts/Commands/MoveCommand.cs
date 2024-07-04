using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class MoveCommand : Command
{
    /// <summary>
    /// Finds the best direction that the dot can turn to in which the
    /// dot it is directed towards can be moved to
    /// </summary>
    /// <param name="t"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public virtual Vector2Int FindBestDirection<T>(T t, Board board)
        where T : IDirectional, IBoardElement
    {

        int rightX = -t.DirectionY;
        int rightY = t.DirectionX;
        int leftX = t.DirectionY;
        int leftY = -t.DirectionX;

        //get the dot that is 90 degrees to the left of the dot (y, -x)
        Dot left = board.Get<Dot>(leftX + t.Column, leftY + t.Row);

        if (CanMove(left))
        {
            return new Vector2Int(leftX, leftY);
        }
        else
        {
            return new Vector2Int(rightX, rightY);

        }

    }

    /// <summary>
    /// Finds the best direction that the dot can turn to in which the
    /// dot it is directed towards can be moved to
    /// </summary>
    /// <param name="t"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public virtual Vector2Int GetRandomDirection<T>(T t, Board board)
        where T : IDirectional, IBoardElement
    {

        List<Dot> neighbors = board.GetNeighbors<Dot>(t.Column, t.Row, false);

        List<Dot> validNeighbors = new();
        foreach(Dot neighbor in neighbors)
        {
            if (CanMove(neighbor))
            {
                validNeighbors.Add(neighbor);
            }
        }
        if(validNeighbors.Count > 0)
        {
            int rand = Random.Range(0, validNeighbors.Count);
            Dot targetDot = validNeighbors[rand];
            return new Vector2Int(targetDot.Column - t.Column, targetDot.Row - t.Row);

        }
        return new Vector2Int(0, 0);



    }


    /// <summary>
    /// Determines if the target dot can be moved to.
    /// </summary>
    /// <param name="targetDot">The dot to check for movement availability.</param>
    /// <returns>True if the dot can be moved to; otherwise, false.</returns>
    public abstract bool CanMove(Dot targetDot);

}
