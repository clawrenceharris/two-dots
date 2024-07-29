using System.Collections;
using UnityEngine;

public class BlockVisualController : TileVisualController, IHittableVisualController
{
    private Block tile;
    private HittableTileVisuals visuals;
    private readonly HittableVisualController hittableVisualController = new();

    public override T GetGameObject<T>() => tile as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Block)dotsGameObject;
        visuals = dotsGameObject.GetComponent<HittableTileVisuals>();
        hittableVisualController.Init(tile, visuals);
        base.Init(dotsGameObject);

    }

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
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