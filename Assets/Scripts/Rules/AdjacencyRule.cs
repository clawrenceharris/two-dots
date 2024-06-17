using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentPositionRule : IComparisonRule<IBoardElement>
{
    public bool Validate(IBoardElement a, IBoardElement b)
    {

        int rowDiff = Mathf.Abs(a.Row - b.Row);
        int colDiff = Mathf.Abs(a.Column - b.Column);
        
        if (!(colDiff > 1 || rowDiff > 1))
        {
            if (!(colDiff > 0 && rowDiff > 0))
            {
                return true;
            }
        }
        

        return false;
    }
}


