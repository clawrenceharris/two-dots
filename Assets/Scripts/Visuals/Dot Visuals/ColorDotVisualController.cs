using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ColorDotVisualController : ConnectableDotVisualController
{
    private new IColorable Dot;
    public override void Init(Dot dot)
    {
        IColorable colorDot = (IColorable)dot;
        Dot = colorDot;
        base.Init((ConnectableDot)Dot);

    }


    public override IEnumerator BombHit()
    {
        SpriteRenderer.color = Color.white;

        return base.BombHit();
    }


    public override void SetColor()
    {
        SpriteRenderer.color = ColorSchemeManager.FromDotColor(Dot.Color);

    }
    public override void AnimateSelectionEffect()
    {

        Visuals.outerDot.color = ColorSchemeManager.FromDotColor(Dot.Color);

        // Scale animation using DOTween
        Visuals.outerDot.transform.DOScale(Vector3.one * 3, DotVisuals.outerDotScaleTime)
            .SetEase(Ease.OutQuad);

        // Alpha animation using DOTween
        Visuals.outerDot.DOFade(0, DotVisuals.outerDotAlphaTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Visuals.outerDot.transform.localScale = Vector2.zero;
                Color color = Visuals.outerDot.color;
                color.a = 1;
                Visuals.outerDot.color = color;
            });
    }

}
