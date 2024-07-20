using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVisualController : TileVisualController, IHittableVisualController
{
    private Water tile;
    private WaterVisuals visuals;
    private readonly HittableVisualController hittableVisualController = new();
    public override T GetGameObject<T>() => tile as T;


    public override T GetVisuals<T>() => visuals as T;
   

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Water)dotsGameObject;
        visuals = dotsGameObject.GetComponent<WaterVisuals>();
        hittableVisualController.Init(tile, visuals);
        base.Init(dotsGameObject);

    }

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = Color.white;
        visuals.water.color = ColorSchemeManager.CurrentColorScheme.water;
    }

    public IEnumerator DoHitAnimation(HitType hitType)
    {
        yield return hittableVisualController.DoHitAnimation(hitType);
    }

    public IEnumerator DoClearAnimation()
    {
        yield return hittableVisualController.DoClearAnimation();
    }
}
