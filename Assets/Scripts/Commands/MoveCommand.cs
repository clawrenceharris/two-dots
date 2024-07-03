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
    /// <param name="dot"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public virtual Vector2Int FindBestDirection<T>(T dot, Board board)
        where T : IDirectional, IBoardElement
    {

        int rightX = -dot.DirectionY;
        int rightY = dot.DirectionX;
        int leftX = dot.DirectionY;
        int leftY = -dot.DirectionX;

        //get the dot that is 90 degrees to the left of the dot (y, -x)
        Dot left = board.Get<Dot>(leftX + dot.Column, leftY + dot.Row);

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
    /// <param name="dot"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public virtual Vector2Int GetRandomDirection<T>(T dot, Board board)
        where T : IDirectional, IBoardElement
    {

        List<Dot> neighbors = board.GetNeighbors<Dot>(dot.Column, dot.Row, false);

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
            return new Vector2Int(targetDot.Column - dot.Column, targetDot.Row - dot.Row);

        }
        return new Vector2Int(0, 0);



    }



    public abstract bool CanMove(Dot dot);

}
