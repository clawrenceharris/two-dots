using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankDotVisualController : BlankDotBaseVisualController
{
    private BlankDot dot;
    public BlankDotVisuals Visuals { get; private set; }

    public override T GetGameObject<T>()
    {
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        base.Init(dotsGameObject);

        dot = (BlankDot)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<BlankDotVisuals>();

        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
    }

    

}
