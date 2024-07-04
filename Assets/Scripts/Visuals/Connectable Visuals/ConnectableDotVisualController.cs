using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Drawing;
using Color = UnityEngine.Color;

public abstract class ConnectableDotVisualController : DotVisualController
{
   
     
    public virtual IEnumerator AnimateSelectionEffect()
    {
        DotVisuals visuals = GetVisuals<DotVisuals>();
        visuals.outerDot.color = color;

        // Scale animation using DOTween
        visuals.outerDot.transform.DOScale(Vector3.one * 3, DotVisuals.outerDotScaleDuration)
        .SetEase(Ease.OutQuad);

        //Animate alpha 
        visuals.outerDot.DOFade(0, DotVisuals.outerDotFadeDuration)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            visuals.outerDot.transform.localScale = Vector2.zero;
            Color color = visuals.outerDot.color;
            color.a = 1;
            visuals.outerDot.color = color;
        });
        yield return null;
    }

    
}
