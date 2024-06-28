using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using static Type;
public class NestingDotVisualController : DotVisualController, IPreviewable
{
    private NestingDotVisuals visuals;
    private NestingDot dot;

  
    public override T GetGameObject<T>()
    {
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (NestingDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<NestingDotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
    }

    protected override void SetColor()
    {
        foreach(Transform child in dot.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) {
                if(child.name != "Nesting Dot Highlight")
                    spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
                
            }
        }
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;

    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        yield break;
    }

    public override IEnumerator HitAnimation(HitType hitType)
    {
        
        if(dot.HitCount < dot.HitsToClear)
        {
            UpdateDotScale();
            DoHitAnimation();

        }

        yield return base.HitAnimation(hitType);

        
    }

    private void DoHitAnimation()
    {

        float duration = 0.5f;
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

    private void UpdateDotScale()
    {
        if (dot.HitCount == 1)
            dot.transform.localScale = Vector2.one * 1.3f;
        else if(dot.HitCount == 2)
        {
            dot.transform.localScale = Vector2.one * 1f;

        }

    }


    public override IEnumerator BombHit()
    {
        //Do nothing

        yield break;
    }

    private IEnumerator DoShakeAnimation()
    {
        float duration = 0.8f;
        float strength = 0.1f; 
        int vibrato = 10; //number of shakes
        float randomness = 20; 
        dot.transform.DOShakePosition(duration, new Vector3(strength, strength, 0), vibrato, randomness, false, true);

        yield return spriteRenderer.DOColor(Color.black, duration);
    }

    public IEnumerator PreviewClear()
    {
        while (dot.HitCount == 2)
        {
            yield return DoShakeAnimation();
            dot.transform.position = new Vector2(dot.Column, dot.Row) * Board.offset;
            yield return new WaitForSeconds(1.5f);

        }
    }
}
