using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectional
{
    int DirectionX { get; set; }
    int DirectionY { get; set; }
    
   /// <summary>
    /// Determines and returns the optimal direction for the directional object to face, 
    /// based on the validity of neighboring game objects in each direction.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <param name="isValidTarget">A function that evaluates the validity of a neighboring game object as a target.</param>
    /// <returns>The optimal direction vector.</returns>
    Vector2Int FindBestDirection(Board board, Func<DotsGameObject, bool> isValidTarget);
    Vector3 ToRotation(int dirX, int dirY);
    void ChangeDirection(int dirX, int dirY);
    
}
