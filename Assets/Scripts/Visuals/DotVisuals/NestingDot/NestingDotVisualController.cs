using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class NestingDotVisualController : DotVisualController
{
    private NestingDotVisuals visuals;
    private NestingDot dot;


    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (NestingDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<NestingDotVisuals>();
        color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        base.Init(dotsGameObject);
    }

    public override void SetInitialColor()
    {
        foreach(Transform child in visuals.spriteRenderer.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) {
                if(child.name != "Highlight")
                    spriteRenderer.color = color;
                
            }
        }
        visuals.spriteRenderer.color = color;

    }
    protected override void SetUp()
    {
        base.SetUp();
        RemoveLayers(0f);
        
    }
   

    
    private void RemoveLayers(float duration)
    {
        for(int i = 0; i < dot.HitCount; i++)
        {

            visuals.nestingDotBottom.transform.DOMoveY(dot.transform.position.y - Board.offset, duration)
            .OnComplete(() =>
            {
                visuals.nestingDotBottom.transform.position = dot.transform.position;
            });

            visuals.nestingDotTop.transform.DOMoveY(dot.transform.position.y + Board.offset, duration)
            .OnComplete(() =>
            {
                visuals.nestingDotTop.transform.position = dot.transform.position;

            });


            visuals.nestingDotBottomSprite.enabled = true;
            visuals.nestingDotTopSprite.enabled = true;

            visuals.nestingDotBottomSprite.DOFade(0, duration);

            visuals.nestingDotBottomSprite.DOFade(0, duration);


        }

    }
    
    public void DecreaseScale()
    {
        if (dot.HitCount == 1)
            dot.transform.localScale = Vector2.one * 1.3f;
        else if(dot.HitCount == 2)
        {
            dot.transform.localScale = Vector2.one * 1f;

        }

    }


    

    public IEnumerator DoIdleAnimation()
    {
        yield break;
    }

   

    
}
