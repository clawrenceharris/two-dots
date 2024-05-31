using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectByPositionRule : IConnectionRule
{
    public bool Validate(ConnectableDot dot)
    {

        LinkedList<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots;
        int rowDiff = Mathf.Abs(dot.Row - connectedDots.Last.Value.Row);
        int colDiff = Mathf.Abs(dot.Column - connectedDots.Last.Value.Column);
        
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
