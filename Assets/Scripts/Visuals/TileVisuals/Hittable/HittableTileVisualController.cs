using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Represents a visual controller base that controls the visuals of a hittable Tile game object
/// </summary>
public abstract class HittableTileVisualController : TileVisualController, IHittableVisualController
{
    private readonly HittableVisualController hittableVisualController = new();

    public override void Init(DotsGameObject dotsGameObject)
    {
       hittableVisualController.Init(this);
       base.Init(dotsGameObject);
    }


    public virtual IEnumerator Hit()
    {
        yield return hittableVisualController.Hit();
    }

    public virtual IEnumerator Clear(float duration)
    {
        yield return hittableVisualController.Clear(duration);
    }

}