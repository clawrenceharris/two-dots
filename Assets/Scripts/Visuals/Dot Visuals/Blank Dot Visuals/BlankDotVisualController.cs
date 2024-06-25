using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
using System;

public class BlankDotVisualController : ConnectableDotVisualController
{
<<<<<<< Updated upstream
=======
    private BlankDot dot;
    
>>>>>>> Stashed changes

    public new BlankDotVisuals Visuals;
    public override void Init(Dot dot)
    {
<<<<<<< Updated upstream
        Visuals = dot.GetComponent<BlankDotVisuals>();

        base.Init(dot);
        
=======
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    private BlankDotVisuals visuals;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BlankDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BlankDotVisuals>();

        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp();
>>>>>>> Stashed changes
    }

    public override IEnumerator Hit(HitType hitType)
    {
        yield return base.Hit(hitType);
        //dot.transform.DOScale(Vector2.zero, GetVisuals<HittableVisuals>().clearDuration);

    }
    protected override void SetColor()
    {
        base.SetColor();
        SpriteRenderer.color = ColorSchemeManager.CurrentColorScheme.blank;
    }
    
    public void SetInnerColor(Color color)
    {
<<<<<<< Updated upstream
       Visuals.innerDot.color = color;
=======
        visuals.innerDot.color = color;
>>>>>>> Stashed changes
    }

    public override void AnimateSelectionEffect()
    {
        
        base.AnimateSelectionEffect();


        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
<<<<<<< Updated upstream
        
        Visuals.innerDot.color = color;
        Visuals.outerDot.color = color;

        Visuals.innerDot.transform.DOScale(Vector2.one, BlankDotVisuals.innerDotScaleTime);
       
=======

        visuals.innerDot.color = color;
        GetVisuals<DotVisuals>().outerDot.color = color;

        visuals.innerDot.transform.DOScale(Vector2.one,visuals.innerDotScaleDuration);

>>>>>>> Stashed changes
    }

    public void AnimateDeselectionEffect()
    {
        visuals.innerDot.transform.DOScale(Vector2.zero, visuals.innerDotScaleDuration);

    }

    



}
