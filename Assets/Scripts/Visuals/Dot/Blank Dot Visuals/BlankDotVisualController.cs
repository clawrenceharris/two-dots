using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankDotVisualController : BlankDotBaseVisualController
{
    private BlankDot dot;
    private BlankDotVisuals visuals;

    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;


    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BlankDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BlankDotVisuals>();
        base.Init(dotsGameObject);

    }


}
