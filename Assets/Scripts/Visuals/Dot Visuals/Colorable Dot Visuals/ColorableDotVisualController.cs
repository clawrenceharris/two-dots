using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class ColorableDotVisualController : ConnectableDotVisualController
{



    public override IEnumerator DoBombHit()
    {
        
        //set the color to the blank color
        SetColor(ColorSchemeManager.CurrentColorScheme.bombLight);

        yield return new WaitForSeconds(HittableVisuals.hitDuration);

        //set the color back to the default color
        SetColor();
        yield return null;
    }
    

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.FromDotColor(GetGameObject<IColorable>().Color);

    }


    public override void AnimateSelectionEffect()
    {
        DotVisuals visuals = GetVisuals<DotVisuals>();
        visuals.outerDot.color = ColorSchemeManager.FromDotColor(GetGameObject<IColorable>().Color);

        visuals.outerDot.transform.DOScale(Vector3.one * 3, DotVisuals.outerDotScaleDuration)
            .SetEase(Ease.OutQuad);

        visuals.outerDot.DOFade(0, DotVisuals.outerDotFadeDuration)
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
