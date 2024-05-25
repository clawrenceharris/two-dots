using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BlankDotVisualController : ConnectableDotVisualController
{

    public new BlankDotVisuals Visuals;
    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BlankDotVisuals>();

        base.Init(dot);
        
    }

    public override void SetColor()
    {
        base.SetColor();
        SpriteRenderer.color = ColorSchemeManager.FromDotColor();
    }
    
    public void SetColor(Color color)
    {
       Visuals.innerDot.color = color;
    }

    public override void AnimateSelectionEffect()
    {
        
        base.AnimateSelectionEffect();


        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        
        Visuals.innerDot.color = color;
        Visuals.outerDot.color = color;

        Visuals.innerDot.transform.DOScale(Vector2.one, BlankDotVisuals.innerDotScaleTime);
       
    }

    public void AnimateDeselectionEffect()
    {
        Visuals.innerDot.transform.DOScale(Vector2.zero, BlankDotVisuals.innerDotScaleTime);

    }




}
