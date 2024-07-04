using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalDotVisualController : ColorableVisualController
{

    private NormalDot dot;
    private DotVisuals visuals;


    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (NormalDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<DotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp();
    }

    

   
}
