using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorableDot : ConnectableDot, IColorable
{
    private DotColor color;
    public DotColor Color { get => color; set => color = value; }

   
}