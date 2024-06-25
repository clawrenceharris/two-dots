using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableBase : IColorable
{
    private IColorable colorable;

    public virtual DotColor Color { get; set; }

    public void Init<T>(T colorable) where T : DotsGameObject, IColorable
    {
        this.colorable = colorable;
    }

}
