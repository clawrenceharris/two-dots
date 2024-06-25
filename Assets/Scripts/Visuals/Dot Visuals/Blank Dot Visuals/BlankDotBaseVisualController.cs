using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
using System;

public abstract class BlankDotBaseVisualController : ConnectableDotVisualController
{
    
    

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.blank;
    }

    public void SetInnerColor(Color color)
    {
        GetVisuals<BlankDotVisuals>().innerDot.color = color;
    }

    public override void AnimateSelectionEffect()
    {
        BlankDotVisuals visuals = GetVisuals<BlankDotVisuals>();
        base.AnimateSelectionEffect();


        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);

        visuals.innerDot.color = color;
        GetVisuals<DotVisuals>().outerDot.color = color;

        visuals.innerDot.transform.DOScale(Vector2.one,visuals.innerDotScaleDuration);

    }

    public void AnimateDeselectionEffect()
    {
        BlankDotVisuals visuals = GetVisuals<BlankDotVisuals>();

        visuals.innerDot.transform.DOScale(Vector2.zero, visuals.innerDotScaleDuration);

    }

}