using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class NestingDotVisualController : DotVisualController
{
    private new NestingDotVisuals Visuals;

    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<NestingDotVisuals>();
        base.Init(dot);
    }

    public override void SetColor()
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
    public IEnumerator Hit()
    {
        UpdateDotScale();
        CoroutineHandler.StartStaticCoroutine(AnimateDotHit());



        while (Dot.HitCount == 2)
        {
            yield return DoShakeAnimation();
            Dot.transform.position = new Vector2(Dot.Column, Dot.Row) * Board.offset;
            yield return new WaitForSeconds(1.5f);

        }



    }

    private IEnumerator AnimateDotHit()
    {
        

        Visuals.nestingDotBottom.transform.DOMoveY(Dot.transform.position.y - Board.offset, 0.5f);
        Visuals.nestingDotTop.transform.DOMoveY(Dot.transform.position.y + Board.offset, 0.5f);
        SpriteRenderer nestingDotBottomSprite = Visuals.nestingDotBottom.GetComponent<SpriteRenderer>();
        SpriteRenderer nestingDotTopSprite = Visuals.nestingDotTop.GetComponent<SpriteRenderer>();
        nestingDotBottomSprite.enabled = true;
        nestingDotTopSprite.enabled = true;
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            nestingDotBottomSprite.color = Color.Lerp(Color, Color.clear, timer / duration);
            nestingDotTopSprite.color = Color.Lerp(Color, Color.clear, timer / duration);
            yield return null;
        }

        Visuals.nestingDotBottom.transform.position = Dot.transform.position;
        Visuals.nestingDotTop.transform.position = Dot.transform.position;

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

        yield return LerpColor();
    }

    private IEnumerator LerpColor()
    {
        float duration = 0.8f;
        float halfDuration = duration / 2f;
        float timer = 0f;

        // Lerp to black
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            SpriteRenderer.color = Color.Lerp(Color, Color.black, timer / halfDuration);
            yield return null;
        }

        SpriteRenderer.color = Color.black;
        timer = 0f;

        // Lerp back to original color
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            SpriteRenderer.color = Color.Lerp(Color.black, Color, timer / halfDuration);
            yield return null;
        }

        // Ensure it's back to the original color
        SpriteRenderer.color = Color;
    }

}
