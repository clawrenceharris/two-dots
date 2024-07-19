using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a hit rule that checks if an IHittable object is at the bottom of the board.
/// </summary>
public class HitByBottomOfBoardRule : IHitRule
{

    public bool Validate(IHittable hittable, Board board)
    {
        if(hittable is IBoardElement boardElement)
            return board.IsAtBottomOfBoard(boardElement.Column, boardElement.Row);
        return false;
    }
}