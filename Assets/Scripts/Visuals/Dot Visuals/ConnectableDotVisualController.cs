using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Drawing;
using Color = UnityEngine.Color;

public class ConnectableDotVisualController : DotVisualController
{

    public new ConnectableDot Dot;


    public override void Init(Dot dot)
    {
        Dot = (ConnectableDot)dot;
        base.Init(Dot);
        

    }

   


    public virtual void AnimateSelectionEffect()
    {

        Visuals.outerDot.color = ColorSchemeManager.CurrentColorScheme.blank;

        // Scale animation using DOTween
        Visuals.outerDot.transform.DOScale(Vector3.one * 3, DotVisuals.outerDotScaleTime)
            .SetEase(Ease.OutQuad);

        //Animate alpha 
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
