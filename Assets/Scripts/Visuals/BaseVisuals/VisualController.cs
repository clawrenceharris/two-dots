using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;
using DG.Tweening;
using Animations;


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

    private DotsAnimator animator;

    public DotsAnimator Animator {
        get{
            if(animator == null){
                if(GetGameObject<DotsGameObject>().TryGetComponent<DotsAnimator>(out var animator)){
                    this.animator = animator;
                }
            }
            return animator;
        }
    }

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

    #region Sprites
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
    #endregion Sprites
    
    #region Animation

    public IEnumerator Animate(IAnimation animation, AnimationLayer layer = AnimationLayer.BaseLayer){
        if(Animator == null){
            yield break;
        }
        yield return GetGameObject<DotsGameObject>().StartCoroutine(Animator.Animate(animation, layer));
    }
   

    public T GetAnimatableComponent<T>(AnimationLayer layer = AnimationLayer.BaseLayer)
    where T : class,  IAnimatable
    {
        if(Animator != null){
            return Animator.GetAnimatableComponent<T>(layer);
        }
        return default;
    }
    

    #endregion Animations
}
