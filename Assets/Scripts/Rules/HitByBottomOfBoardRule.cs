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
        bool isAtBottom = board.IsAtBottomOfBoard(hittable.Column, hittable.Row);

        return isAtBottom;

    }
}