using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;
using DG.Tweening;


/// <summary>
/// Represents a visual controller that controlls the visuals of a Dots game object
/// </summary>
public abstract class VisualController : IVisualController
{
    protected Color color;

    /// <summary>
    /// Retrieves the <see cref="Visuals"/> component attached to the 
    /// <see cref="DotsGameObject"/> that this visual controller manages.
    /// </summary>
    /// <typeparam name="T">A reference type/></typeparam>
    /// <returns>A Visuals game object of type <typeparamref name="T"/>.</returns>
    public abstract T GetVisuals<T>() where T : class;

    /// <summary>
    /// Retrieves the game object managed by this visual controller and attempts to 
    /// cast it to the specified type <typeparamref name="T"/>. 
    /// If the game object cannot be cast to the specified type, it returns <c>null</c>.
    /// </summary>
    /// <typeparam name="T">The type to which the managed game object should be cast.</typeparam>
    /// <returns>An object of type <typeparamref name="T"/> if the cast is successful; otherwise, <c>null</c>.</returns>
    public abstract T GetGameObject<T>() where T : class;

   

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

    public bool TryGetVisuals<T>(out T visuals)
        where T : class
    {
        visuals = GetVisuals<T>();
        if (visuals != null)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    protected DotsAnimator animator;
    protected Sprite sprite;

    public abstract void Init(DotsGameObject dotsGameObject);
    
    public abstract void SetInitialColor();

    /// <summary>
    /// Performs set up operations such as setting
    /// the initial color, scale and rotation 
    /// </summary>
    protected virtual void SetUp()
    {
        SetInitialColor();

    }

    protected Coroutine StartCoroutine(IEnumerator coroutine,Action onComplete = null){
        return CoroutineHandler.StartStaticCoroutine(coroutine, onComplete);
    }

    public virtual void SetColor(Color color)
    {
        GetVisuals<Visuals>().spriteRenderer.color = color;
    }

    

    /// <summary>
    /// Disables all sprite renderers on the game object
    /// </summary>
    public virtual void DisableSprites()
    {
        GetVisuals<Visuals>().spriteRenderer.enabled = false;
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
        GetVisuals<Visuals>().spriteRenderer.enabled = true;
        foreach (Transform child in GetGameObject<DotsGameObject>().transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = true;
            }
        }
    }

    public T GetDotsGameObject<T>()
    {
        throw new NotImplementedException();
    }

    public IEnumerator Animate(IAnimation animation, AnimationLayer layer = AnimationLayer.BaseLayer){
        if(animator == null){
            yield break;
        }
        yield return animator.Animate(animation, layer);
    }
}
