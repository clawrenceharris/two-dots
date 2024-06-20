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

    public virtual IEnumerator Hit(HitType hitType)
    {
        if (hitType == HitType.BombExplosion)
        {
            yield return BombHit();
        }
    }

    public virtual IEnumerator Clear()
    {
       yield return  GetGameObject<DotsGameObject>().transform.DOScale(Vector2.zero,
            GetVisuals<HittableVisuals>().clearDuration);
       
    }


}
