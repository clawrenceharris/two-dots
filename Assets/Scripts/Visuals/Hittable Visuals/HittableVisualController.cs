using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using DG.Tweening;

/// <summary>
/// Represents a visual controller that controlls the visuals of a hittable Dots game object
/// </summary>
public abstract class HittableVisualController : VisualController, IHittableVisualController
{
    

    public virtual IEnumerator BombHit()
    {
        GetVisuals<HittableVisuals>().bombHitSprite.color = ColorSchemeManager.CurrentColorScheme.bombLight;
        spriteRenderer.sprite = GetVisuals<HittableVisuals>().bombHitSprite.sprite;

        yield return new WaitForSeconds(HittableVisuals.defaultClearDuration);

        spriteRenderer.sprite = sprite;
    }


    /// <summary>
    /// Starts the hit animation for the
    /// Dots game object associated with this visual controller.
    /// The default hit animation looks the the default clear animation,
    /// which scales out the game object
    /// </summary>
    /// <returns>IEnumerator</returns>
    public virtual IEnumerator HitAnimation(HitType hitType)
    {
        if (GetGameObject<IHittable>().HitCount >= GetGameObject<IHittable>().HitsToClear)
        {
            yield return GetGameObject<DotsGameObject>().transform.DOScale(Vector2.zero,
        GetVisuals<HittableVisuals>().clearDuration);
        }
        
    }


    public virtual IEnumerator ClearAnimation()
    {
        yield return GetGameObject<DotsGameObject>().transform.DOScale(Vector2.zero,
             GetVisuals<HittableVisuals>().clearDuration);
       
    }

    
    
}
