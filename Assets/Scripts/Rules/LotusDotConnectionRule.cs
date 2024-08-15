using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LotusDotConnectionRule : IComparisonRule<IConnectable>
{
    /// <summary>
    /// Checks if a given lotus dot can be connected to another given dot.
    /// A lotus dot can be connected to if the other dot is the same color or is a Moonstone dot
    /// </summary>
    /// <param name="a">The Lotus dot</param>
    /// <param name="b">The other dot to compare</param>
    /// <param name="board">The game board</param>
    /// <returns>Whether the lotus dot can be connected to</returns>
    public bool Validate(IConnectable a, IConnectable b, Board board)
    {
        if(a.Color == b.Color)
        {
            return true;
        }
        if (b is Dot dot && dot.DotType.IsMoonstoneDot())
        {
            return true;
        }
        return false;   
        
    }
}
