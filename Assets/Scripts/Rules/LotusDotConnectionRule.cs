using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusDotConnectionRule : ConnectionRule
{
    /// <summary>
    /// Checks if a given lotus dot can connect to another given dot.
    /// A lotus dot can be connected if if the other dot is the same color or it is a Moonstone dot
    /// </summary>
    /// <param name="a">The lotus dot</param>
    /// <param name="b">The other dot</param>
    /// <param name="board">The game board</param>
    /// <returns>Whether the lotus dot can be connected to</returns>
    public override bool Validate(ConnectableDot a, ConnectableDot b, Board board = null)
    {
        if(a.Color == b.Color)
        {
            return true;
        }
        if (b.DotType.IsMoonstoneDot())
        {
            return true;
        }
        return false;
    }
}
