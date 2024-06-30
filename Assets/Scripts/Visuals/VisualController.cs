using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;


/// <summary>
/// Represents a visual controller that controlls the visuals of a Dots game object
/// </summary>
public abstract class VisualController : IVisualController
{
    protected SpriteRenderer spriteRenderer;
    protected Color color;

    /// <summary>
    /// Returns the Visuals game object that is attached to
    /// the Dots game object of which this visual controller acts on
    /// </summary>
    /// <typeparam name="T">A game object of type Visuals</typeparam>
    /// <returns>The Visuals game object</returns>
    public abstract T GetVisuals<T>()
        where T : class;

    /// <summary>
    /// Returns the Two Dots game object that this visual
    /// controller is associated with
    /// </summary>
    /// <typeparam name="T">A game object of type DotsGameObject</typeparam>
    /// <returns>The Two Dots game object</returns>
    public abstract T GetGameObject<T>() where T : class;

    public DotsGameObject GetGameObject()
    {
        return GetGameObject<DotsGameObject>();
    }

    public bool TryGetGameObject<T>(out T dotsGameObject)
        where T : class
    {
        dotsGameObject = GetGameObject<T>();
        if (dotsGameObject != null)
        {
            return true;

        }
        else
        {
            return false;
        }
    }


    protected Sprite sprite;


    public abstract void Init(DotsGameObject dotsGameObject);
    
    protected abstract void SetColor();

    /// <summary>
    /// Performs set up operations such as setting
    /// the initial color, scale and rotation 
    /// </summary>
    protected virtual void SetUp()
    {
        SetColor();

    }


    public virtual void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }


    /// <summary>
    /// Disables all sprite renderers on the game object
    /// </summary>
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


    /// <summary>
    /// Enables all sprite renderers on the game object
    /// </summary>
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
