using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using static Type;
public class NestingDotVisualController : DotVisualController
{
    private new NestingDotVisuals Visuals;

    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<NestingDotVisuals>();
        base.Init(dot);
    }

    protected override void SetColor()
    {
        foreach(Transform child in Dot.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) {
                if(child.name != "Nesting Dot Highlight")
                    spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
                
            }
        }
        SpriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;

        base.SetColor();
    }

    public override IEnumerator PreviewHit(HitType hitType)
    {
        while (Dot.HitCount == 2)
        {
            yield return DoShakeAnimation();
            Dot.transform.position = new Vector2(Dot.Column, Dot.Row) * Board.offset;
            yield return new WaitForSeconds(1.5f);

        }
        yield return base.PreviewHit(hitType);
    }

    public override IEnumerator Hit(HitType hitType)
    {
        
        if(Dot.HitCount < Dot.HitsToClear)
        {
            UpdateDotScale();
            DoHitAnimation();

        }

        yield return base.Hit(hitType);

        
    }

    private void DoHitAnimation()
    {

        float duration = 0.5f;
        Visuals.nestingDotBottom.transform.DOMoveY(Dot.transform.position.y - Board.offset, duration)
            .OnComplete(() =>
            {
                Visuals.nestingDotBottom.transform.position = Dot.transform.position;
            });
        Visuals.nestingDotTop.transform.DOMoveY(Dot.transform.position.y + Board.offset, duration)
            .OnComplete(() =>
            {
                Visuals.nestingDotTop.transform.position = Dot.transform.position;

            });

        Visuals.nestingDotBottomSprite.enabled = true;
        Visuals.nestingDotTopSprite.enabled = true;

        Visuals.nestingDotBottomSprite.DOFade(0, duration);

        Visuals.nestingDotBottomSprite.DOFade(0, duration);
            

    }

    private void UpdateDotScale()
    {
        if (Dot.HitCount == 1)
            Dot.transform.localScale = Vector2.one * 1.3f;
        else if(Dot.HitCount == 2)
        {
            Dot.transform.localScale = Vector2.one * 1f;

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
        Dot.transform.DOShakePosition(duration, new Vector3(strength, strength, 0), vibrato, randomness, false, true);

        yield return SpriteRenderer.DOColor(Color.black, duration);
    }

    

}
