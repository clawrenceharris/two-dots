using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingColorRule : IComparisonRule<DotColor>
{
    /// <summary>
    /// Checks whether two dot colors match
    /// </summary>
    /// <param name="a">The dot in the connection</param>
    /// <param name="b">The dot that is not in the connection yet</param>
    /// <returns></returns>
    public bool Validate(DotColor a, DotColor b, Board board)
    {

        return a == b || b == DotColor.Blank || a == DotColor.Blank;      
    }
}
