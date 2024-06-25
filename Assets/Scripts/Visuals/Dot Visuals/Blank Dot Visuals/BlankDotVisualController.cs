using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
using System;

public class BlankDotVisualController : ConnectableDotVisualController
{
    private BlankDot dot;
    

    public override T GetGameObject<T>()
    {
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    private BlankDotVisuals visuals;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BlankDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BlankDotVisuals>();

        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp();
    }

    public override IEnumerator Hit(HitType hitType)
    {
        yield return base.Hit(hitType);
        //dot.transform.DOScale(Vector2.zero, GetVisuals<HittableVisuals>().clearDuration);

    }
    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.blank;
    }

    public void SetInnerColor(Color color)
    {
        visuals.innerDot.color = color;
    }

    public override void AnimateSelectionEffect()
    {

        base.AnimateSelectionEffect();


        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);

        visuals.innerDot.color = color;
        GetVisuals<DotVisuals>().outerDot.color = color;

        visuals.innerDot.transform.DOScale(Vector2.one,visuals.innerDotScaleDuration);

    }

    public void AnimateDeselectionEffect()
    {
        visuals.innerDot.transform.DOScale(Vector2.zero, visuals.innerDotScaleDuration);

    }

}