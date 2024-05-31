using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectional : IBoardElement
{
    int DirectionX { get; set; }
    int DirectionY { get; set; }
}
