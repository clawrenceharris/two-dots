using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankDotVisualController : BlankDotBaseVisualController
{
    private BlankDot dot;
    private BlankDotVisuals visuals;

    public override T GetGameObject<T>()
    {
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BlankDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BlankDotVisuals>();

        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp();
    }

    

}
