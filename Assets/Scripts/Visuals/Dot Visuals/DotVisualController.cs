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
    public virtual void SetUp()
    {
        SetColor();
    }

    public virtual void SetColor()
    { 
        Color = SpriteRenderer.color;
    }

   

    public virtual IEnumerator BombHit()
    {   
        SpriteRenderer.sprite = Visuals.bombHitSprite;

        yield return new WaitForSeconds(Visuals.clearTime);

        SpriteRenderer.sprite = sprite;

        
        
    }

    public virtual IEnumerator Hit(HitType hitType)
    {
        yield return null;

    }

    public void SetColor(Color color)
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
        Dot.transform.DOScale(Vector2.zero, Visuals.clearTime);
        yield return new WaitForSeconds(Visuals.clearTime);
    }

   
}
