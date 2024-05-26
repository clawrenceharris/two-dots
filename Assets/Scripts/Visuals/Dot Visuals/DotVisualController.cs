using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class DotVisualController : IDotVisualController
{

    public virtual Dot Dot { get; protected set; }
    public DotVisuals Visuals { get; protected set; }

    public SpriteRenderer SpriteRenderer { get; protected set; }
    private Color color;
    private Sprite sprite;
    

    public virtual void Init(Dot dot)
    {
        Dot = dot;
        Visuals = dot.GetComponent<DotVisuals>();
        SpriteRenderer = dot.GetComponent<SpriteRenderer>();
        color = SpriteRenderer.color;
        sprite = SpriteRenderer.sprite;
        SetUp();
    }
    public virtual void SetUp()
    {
        SetColor();
    }
    public virtual void SetColor()
    {
        SpriteRenderer.color = color;
        
    }

    public void ActivateBomb()
    {
        SpriteRenderer.enabled = false;
        DisableChildren();
    }

    public void DeactivateBomb()
    {
        SpriteRenderer.enabled = true;
        EnableChildren();
    }

    private void EnableChildren()
    {
        foreach (Transform child in Dot.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void DisableChildren()
    {
        foreach(Transform child in Dot.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public virtual IEnumerator BombHit()
    {
        SpriteRenderer.sprite = Visuals.bombHitSprite;
 
        yield return new WaitForSeconds(Visuals.clearTime);

        SpriteRenderer.sprite = sprite;
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
