using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMulticolorable : IColorable
{
    public DotColor[] Colors { get; set; }
}
