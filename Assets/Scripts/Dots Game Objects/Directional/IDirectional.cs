using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectional
{
    int DirectionX { get; set; }
    int DirectionY { get; set; }
    void ChangeDirection(int directionX, int directionY);
    Vector3 GetRotation();
}
