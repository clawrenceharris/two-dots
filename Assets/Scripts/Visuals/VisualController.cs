using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;



/// <summary>
/// Represents a visual controller that controlls the visuals of a Dots game object
/// </summary>
public abstract class VisualController : IVisualController
{
    protected SpriteRenderer spriteRenderer;
    protected Color color;


    public abstract T GetVisuals<T>() where T : Visuals;
    public abstract T GetGameObject<T>() where T : DotsGameObject;


    protected Sprite sprite;


    public abstract void Init(DotsGameObject dotsGameObject);
    
    protected abstract void SetColor();

    
    protected virtual void SetUp()
    {
        SetColor();

    }


    public virtual void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }


    public virtual void DisableSprites()
    {
        spriteRenderer.enabled = false;
        foreach (Transform child in GetGameObject<DotsGameObject>().transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    public virtual void EnableSprites()
    {
        spriteRenderer.enabled = true;
        foreach (Transform child in GetGameObject<DotsGameObject>().transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = true;
            }
        }
    }

}
