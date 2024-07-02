using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using static Type;
public abstract class DotVisualController : HittableVisualController, IDotVisualController
{
    

    public IEnumerator Pulse()
    {
        throw new NotImplementedException();
    }
}
