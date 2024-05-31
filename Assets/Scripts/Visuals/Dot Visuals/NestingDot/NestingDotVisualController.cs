using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NestingDotVisualController : DotVisualController
{
   
    public override void SetColor()
    {
        SpriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        base.SetColor();
    }
    public IEnumerator Hit()
    {
        if(Dot.HitCount == 1)
            Dot.transform.localScale = Vector2.one * 1.5f;
        else
        {
            Dot.transform.localScale = Vector2.one * 1f;

        }

        while (Dot.HitCount == 2)
        {
            yield return DoShakeAnimation();
            Dot.transform.position = new Vector2(Dot.Column, Dot.Row) * Board.offset;
            yield return new WaitForSeconds(1.5f);

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

        // Ensure it's fully black
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
