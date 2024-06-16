using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using static Type;
public class DotVisualController : IDotVisualController
{

    public virtual Dot Dot { get; protected set; }
    public DotVisuals Visuals { get; protected set; }

    public SpriteRenderer SpriteRenderer { get; protected set; }
    public Color Color { get; private set; }
    private Sprite sprite;
    

    public virtual void Init(Dot dot)
    {
        Dot = dot;
        Visuals = dot.GetComponent<DotVisuals>();
        SpriteRenderer = dot.GetComponent<SpriteRenderer>();
        sprite = SpriteRenderer.sprite;
        SetUp();
    }

    protected virtual void SetUp()
    {
        SetColor();
    }
    public virtual void ActivateBomb() {
        DisableSprites();
    }
    protected virtual void SetColor()
    { 
        Color = SpriteRenderer.color;
    }

   

    public virtual IEnumerator BombHit()
    {
        Visuals.bombHitSprite.color = ColorSchemeManager.CurrentColorScheme.bombLight;
        SpriteRenderer.sprite =  Visuals.bombHitSprite.sprite;

        yield return new WaitForSeconds(DotVisuals.defaultClearDuration);

        SpriteRenderer.sprite = sprite;

        
        
    }

    public virtual IEnumerator Hit(HitType hitType)
    {
        if(hitType == HitType.BombExplosion)
        {
            yield return BombHit();
        }


    }

    public virtual void SetColor(Color color)
    {
        SpriteRenderer.color = color;

        foreach (Transform child in Dot.transform)
        {
            if (child.TryGetComponent(out SpriteRenderer sr))
            {

                sr.color = color;
            }
        }
    }

    public virtual IEnumerator Clear()
    {
        Dot.transform.DOScale(Vector2.zero, Visuals.clearDuration);
        yield return new WaitForSeconds(Visuals.clearDuration);
    }

    public virtual IEnumerator PreviewHit(HitType hitType)
    {
        yield return null;
    }

    public virtual void DisableSprites()
    {
        SpriteRenderer.enabled = false;
        foreach(Transform child in Dot.transform)
        {
            if(child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    public virtual void EnableSprites()
    {
        SpriteRenderer.enabled = true;
        foreach (Transform child in Dot.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = true;
            }
        }
    }
}
