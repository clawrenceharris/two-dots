using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class ConnectionRule : IComparisonRule<ConnectableDot>
{

    /// <summary>
    /// Checks whether the connection of two dots is valid
    /// </summary>
    /// <param name="a">The dot in the connection</param>
    /// <param name="b">The dot that is not in the connection yet</param>
    /// <returns></returns>
    public bool Validate(ConnectableDot a, ConnectableDot b, Board board = null)
    {
        Connection connection = ConnectionManager.Connection;
        MatchingColorRule colorRule = new();
        AdjacentPositionRule adjacentRule = new();
        
        if ( b is IColorable d )
        {
            if (!colorRule.Validate(connection.Color, d.Color, board))
            {
                return false;
            }
        }
        
        

        if(!adjacentRule.Validate(a, b))
        {
            return false;
        }

        return true;
    }
}
