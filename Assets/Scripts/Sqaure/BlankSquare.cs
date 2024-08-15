using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankSquare : Square
{
    public BlankSquare(Board board) : base(board)
    {
    }

    protected override bool ShouldHitDot(Dot dot)
    {
        return dot is IColorable;
    }
   

}
