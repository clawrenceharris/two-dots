using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INumerableVisualController
{
    public void UpdateNumbers(int amount);

    public IEnumerator ScaleNumbers();
}