using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultiColorable : IColorable
{
    public DotColor[] Colors { get; set; }
}
