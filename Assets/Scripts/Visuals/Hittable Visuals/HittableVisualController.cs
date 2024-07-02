using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;

/// <summary>
/// Represents a visual controller that controlls the visuals of a hittable Dots game object
/// </summary>
public abstract class HittableVisualController : VisualController
{



    /// <summary>
    /// Starts the coroutine to visually
    /// indicate that a hittable has been hit by a bomb 
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator DoBombHit()
    {
        IHittableVisuals visuals = GetVisuals<IHittableVisuals>();
        visuals.BombHitSprite.color = ColorSchemeManager.CurrentColorScheme.bombLight;
        spriteRenderer.sprite = visuals.BombHitSprite.sprite;

        yield return new WaitForSeconds(HittableVisuals.bombHitDuration);

        spriteRenderer.sprite = sprite;
    }



    public virtual IEnumerator DoHitAnimation(HitType hitType)
    {
        yield return null;
    }

    public virtual IEnumerator DoClearAnimation()
    {
        IHittableVisuals visuals = GetVisuals<IHittableVisuals>();
        DotsGameObject dotsGameObject = GetGameObject<DotsGameObject>();
        yield return dotsGameObject.transform.DOScale(Vector2.zero, visuals.ClearDuration);
       
    }

    
    
}
