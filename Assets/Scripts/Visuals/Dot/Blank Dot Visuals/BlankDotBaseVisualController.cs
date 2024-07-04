using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
using System;
using System.Linq;

public abstract class BlankDotBaseVisualController : ColorableVisualController, IPreviewable
{
    
    

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.blank;
    }

    public void SetInnerColor(Color color)
    {
        GetVisuals<BlankDotVisuals>().innerDot.color = color;
    }

    public override IEnumerator AnimateSelectionEffect()
    {
        base.AnimateSelectionEffect();
        BlankDotVisuals visuals = GetVisuals<BlankDotVisuals>();


        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);

        visuals.innerDot.color = color;
        GetVisuals<DotVisuals>().outerDot.color = color;

        yield return visuals.innerDot.transform.DOScale(Vector2.one, BlankDotVisuals.innerDotScaleDuration);

    }

    public IEnumerator AnimateDeselectionEffect()
    {
        BlankDotVisuals visuals = GetVisuals<BlankDotVisuals>();

        yield return visuals.innerDot.transform.DOScale(Vector2.zero, BlankDotVisuals.innerDotScaleDuration);

    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        BlankDotBase dot = GetGameObject<BlankDotBase>();
        while (ConnectionManager.ConnectedDots.Contains(dot))
        {
            dot.UpdateColor();

            yield return AnimateSelectionEffect();
        }

        yield return AnimateDeselectionEffect();

    }

    public IEnumerator PreviewClear()
    {
        throw new NotImplementedException();
    }
}