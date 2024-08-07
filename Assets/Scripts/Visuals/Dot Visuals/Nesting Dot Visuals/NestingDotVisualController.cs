using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class NestingDotVisualController : DotVisualController, IPreviewableVisualController
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
        foreach(Transform child in dot.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) {
                if(child.name != "Nesting Dot Highlight")
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
   

    public override IEnumerator Hit(HitType hitType)
    {
        
        if(dot.HitCount < dot.HitsToClear)
        {
            float duration = visuals.hittableVisuals.HitDuration;
            RemoveLayers(duration);
            
            yield return null;
        }
        
    }
    private void RemoveLayers(float duration)
    {
        for(int i = 0; i < dot.HitCount; i++)
        {
            UpdateDotScale();

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
    
    private void UpdateDotScale()
    {
        if (dot.HitCount == 1)
            dot.transform.localScale = Vector2.one * 1.3f;
        else if(dot.HitCount == 2)
        {
            dot.transform.localScale = Vector2.one * 1f;

        }

    }


    
    private IEnumerator DoShakeAnimation()
    {
        float duration = 0.8f;
        float strength = 0.1f; 
        int vibrato = 10; //number of shakes
        float randomness = 20; 
        dot.transform.DOShakePosition(duration, new Vector3(strength, strength, 0), vibrato, randomness, false, true);

        visuals.spriteRenderer.DOColor(Color.black, duration);
        yield return new WaitForSeconds(duration);
    }

    

    public IEnumerator DoIdleAnimation()
    {
        yield break;
    }

    public IEnumerator DoHitPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoClearPreviewAnimation()
    {
        float duration = 0.8f;
        yield return DoShakeAnimation();
        dot.transform.position = new Vector2(dot.Column, dot.Row) * Board.offset;
        yield return visuals.spriteRenderer.DOColor(color, duration);
        yield return new WaitForSeconds(2f);
    }
}
