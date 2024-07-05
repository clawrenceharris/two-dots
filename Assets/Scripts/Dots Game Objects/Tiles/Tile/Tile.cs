using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static Type;
public abstract class Tile : DotsGameObject
{
    
    public abstract TileType TileType { get;}
    public new VisualController VisualController => GetVisualController<VisualController>();
}
