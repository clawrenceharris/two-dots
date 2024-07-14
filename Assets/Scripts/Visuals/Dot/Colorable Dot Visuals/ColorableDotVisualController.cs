using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
public abstract class ColorableDotVisualController : DotVisualController
{

    public virtual IEnumerator AnimateSelectionEffect()
    {
        DotVisuals visuals = GetVisuals<DotVisuals>();

        DotColor dotColor = GetGameObject<IColorable>().Color;
        visuals.outerDot.color = ColorSchemeManager.FromDotColor(dotColor);

        // Animate scale
        visuals.outerDot.transform.DOScale(Vector3.one * 3, DotVisuals.selectionAnimationDuration)
        .SetEase(Ease.OutQuad);

        //Animate alpha 
        visuals.outerDot.DOFade(0, DotVisuals.selectionAnimationDuration)
        .SetEase(Ease.Linear);

        yield return new WaitForSeconds(DotVisuals.selectionAnimationDuration);

        visuals.outerDot.transform.localScale = Vector2.zero;
        Color color = visuals.outerDot.color;
        color.a = 1;
        visuals.outerDot.color = color;

        
    }

    
}
