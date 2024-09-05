using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;


public abstract class DotVisualController : VisualController, IDotVisualController

{

    public override void Init(DotsGameObject dotsGameObject)
    {
        Animator = dotsGameObject.GetComponent<DotsAnimator>();
        SetUp();

    }

    
    public IEnumerator Pulse()
    {
        throw new NotImplementedException();
    }

    
}
