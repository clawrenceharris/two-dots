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
    private HittableVisuals visuals;

    public override void Init(DotsGameObject dotsGameObject)
    {
        visuals = dotsGameObject.GetComponent<HittableVisuals>();
    }



    /// <summary>
    /// Starts the coroutine to visually
    /// indicate that a hittable has been hit by a bomb 
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator DoBombHit()
    {
        visuals.bombHitSprite.color = ColorSchemeManager.CurrentColorScheme.bombLight;
        spriteRenderer.sprite = GetVisuals<HittableVisuals>().bombHitSprite.sprite;

        yield return new WaitForSeconds(HittableVisuals.hitDuration);

        spriteRenderer.sprite = sprite;
    }



    public virtual IEnumerator DoHitAnimation(HitType hitType)
    {
        yield return null;
    }

    public virtual IEnumerator DoClearAnimation()
    {
        yield return GetGameObject<DotsGameObject>().transform.DOScale(Vector2.zero,
             visuals.clearDuration);
       
    }

    
    
}
