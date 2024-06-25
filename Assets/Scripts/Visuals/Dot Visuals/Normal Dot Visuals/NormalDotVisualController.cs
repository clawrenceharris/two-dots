using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalDotVisualController : ColorDotVisualController
{
<<<<<<< Updated upstream
=======

    private NormalDot dot;
    private DotVisuals visuals;


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
        dot = (NormalDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<DotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp();
    }

>>>>>>> Stashed changes
    public override IEnumerator Hit(Type.HitType hitType)
    {
        yield return base.Hit(hitType);
        Dot.transform.DOScale(Vector2.zero, Visuals.clearDuration);

    }
}
