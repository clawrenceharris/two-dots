using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalDotVisualController : ColorableDotVisualController
{

    private NormalDot dot;
    public DotVisuals Visuals { get; private set; }


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

        dot = (NormalDot)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<DotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp();
    }

    

   
}
