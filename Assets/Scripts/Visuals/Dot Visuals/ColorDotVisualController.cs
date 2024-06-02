using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ColorDotVisualController : ConnectableDotVisualController
{
    protected new ColorableDot Dot;
    public override void Init(Dot dot)
    {
        ColorableDot colorDot = (ColorableDot)dot;
        Dot = colorDot;
        base.Init(Dot);

    }


    public override IEnumerator BombHit()
    {
        if(Dot.gameObject == null)
        {
            yield break;
        }
        //set the color to the blank color
        SetColor(ColorSchemeManager.CurrentColorScheme.blank);


        //set the color back to the default color
        SetColor();
        yield return null;
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
