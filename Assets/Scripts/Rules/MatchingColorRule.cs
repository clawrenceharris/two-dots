using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingColorRule : IComparisonRule<IColorable>
{
    public bool Validate(IColorable a, IColorable b, Board board)
    {

        return a.Color == b.Color;      
    }
}
