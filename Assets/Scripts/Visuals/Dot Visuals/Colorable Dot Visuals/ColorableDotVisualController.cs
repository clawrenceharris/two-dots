using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class ColorableDotVisualController : ConnectableDotVisualController
{



    public override IEnumerator BombHit()
    {
        
        //set the color to the blank color
        SetColor(ColorSchemeManager.CurrentColorScheme.bombLight);


        //set the color back to the default color
        SetColor();
        yield return null;
    }
    

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.FromDotColor(GetGameObject<ColorableDot>().Color);

    }
    public override void AnimateSelectionEffect()
    {
        DotVisuals visuals = GetVisuals<DotVisuals>();
        visuals.outerDot.color = ColorSchemeManager.FromDotColor(GetGameObject<ColorableDot>().Color);

        // Scale animation using DOTween
        visuals.outerDot.transform.DOScale(Vector3.one * 3, DotVisuals.outerDotScaleTime)
            .SetEase(Ease.OutQuad);

        // Alpha animation using DOTween
        visuals.outerDot.DOFade(0, DotVisuals.outerDotAlphaTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                visuals.outerDot.transform.localScale = Vector2.zero;
                Color color = visuals.outerDot.color;
                color.a = 1;
                visuals.outerDot.color = color;
            });
    }

}
