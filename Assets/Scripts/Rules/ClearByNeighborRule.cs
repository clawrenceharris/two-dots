using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class HitByNeighborRule : IHitRule
{

    public bool Validate(IHittable hittable,Board board)
    {
        if(hittable is not  IDirectional directional)
        {
            return false;
        }
        int targetCol = directional.Column + directional.DirectionX;
        int targetRow = directional.Row + directional.DirectionY;

<<<<<<< Updated upstream:Assets/Scripts/Rules/ClearByNeighborRule.cs
        ConnectableDot adjacentDot = board.GetBoardElementDotAt<ConnectableDot>(targetCol, targetRow);
        return ConnectionManager.ConnectedDots.Contains(adjacentDot);
=======
        ConnectableDot adjacentDot = board.Get<ConnectableDot>(targetCol, targetRow);
        
         
        return ConnectionManager.GetElementsToHit<ConnectableDot>().Contains(adjacentDot);
>>>>>>> Stashed changes:Assets/Scripts/Rules/HitByNeighborRule.cs
        

    }
}
