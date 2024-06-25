using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

/// <summary>
/// Represents a hit rule that checks if an IHittable object is at the bottom of the board.
/// </summary>
public class HitByBottomOfBoardRule : IHitRule
{

    public bool Validate(IHittable hittable, Board board)
    {
<<<<<<< Updated upstream
        bool isAtBottom = board.IsAtBottomOfBoard(hittable.Column, hittable.Row);

        return isAtBottom;

=======
        if(hittable is IBoardElement boardElement)
            return board.IsAtBottomOfBoard(boardElement.Column, boardElement.Row);
        return false;
>>>>>>> Stashed changes
    }
}