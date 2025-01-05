using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalBase : IDirectional
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }
    private IDirectional directional;
    public DotsGameObject DotsGameObject => (DotsGameObject)directional;
    public T GetGameObject<T>() where T : DotsGameObject => (T)directional;

    public IDirectionalVisualController VisualController => DotsGameObject.GetVisualController<IDirectionalVisualController>();

    /// <summary>
    /// Converts the given x direction and y direction to the correct rotation value as a Vector
    /// </summary>
    /// <param name="dirX">The x direction</param>
    /// <param name="dirY">The y direction</param>
    /// <returns></returns>
    public Vector3 ToRotation(int dirX, int dirY)
    {
        Vector3 rotation = Vector3.zero;
        if (dirY < 0)
        {
            rotation = new Vector3(0, 0, 180);
        }

        if (dirX < 0)
        {
            rotation = new Vector3(0, 0, 90);

        }
        if (dirX > 0)
        {
            rotation = new Vector3(0, 0, -90);

        }
        return rotation;
    }

    public void Init(IDirectional directional)
    {
        this.directional = directional;
    }
    /// <summary>
    /// Finds and returns the best direction vector for the directional object to face (either left or right) 
    /// depending on if the neighboring game object in that direction is valid. If no direction points to a valid neighbor,
    /// turns right by default.
    /// </summary>
    /// <param name="board">The game board object</param>
    /// <param name="isValidTarget">A function that returns the validity of target game object that was found</param>
    /// <returns>The best direction. Right if none was found.</returns>
    public Vector2Int FindBestDirection(Board board, Func<DotsGameObject, bool> isValidTarget){
        Vector2Int right = new(-DirectionY,DirectionX );
        Vector2Int left = new(DirectionY, -DirectionX);

        //get the dot that is 90 degrees to the left of the dot (y, -x)
        Dot targetDot = board.GetDotAt(left.x + DotsGameObject.Column, left.y + DotsGameObject.Row);

        if (isValidTarget(targetDot))
        {
            return left;
        }
        else
        {
            return right;

        }
    }

    public void ChangeDirection(int dirX, int dirY)
    {
        DirectionX = Mathf.Clamp(dirX, -1, 1);
        DirectionY = Mathf.Clamp(dirY, -1, 1);        
    }

    
}
