using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class IceVisualController : TileVisualController, IHittableVisualController
{
    private Ice tile;
    private IceVisuals visuals;
    private readonly HittableVisualControllerBase hittableVisualController = new();
    public override T GetGameObject<T>() => tile as T;
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Ice)dotsGameObject;
        visuals = dotsGameObject.GetComponent<IceVisuals>();
        hittableVisualController.Init(tile, visuals);
        SetUp();
    }

    protected override void SetColor()
    {
        //do nothing
    }

    public IEnumerator DoBombHit()
    {
        yield return hittableVisualController.DoBombHit();
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
