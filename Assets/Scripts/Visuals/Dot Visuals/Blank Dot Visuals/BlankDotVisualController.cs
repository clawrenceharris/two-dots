using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;

public class BlankDotVisualController : ConnectableDotVisualController
{

    public new BlankDotVisuals Visuals;
    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BlankDotVisuals>();

        base.Init(dot);
        
    }

    public override IEnumerator Hit(HitType hitType)
    {
        yield return base.Hit(hitType);
        Dot.transform.DOScale(Vector2.zero, Visuals.clearDuration);

    }
    protected override void SetColor()
    {
        base.SetColor();
        SpriteRenderer.color = ColorSchemeManager.CurrentColorScheme.blank;
    }
    
    public void SetInnerColor(Color color)
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
