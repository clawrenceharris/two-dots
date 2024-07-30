using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitVisualController : TileVisualController, IHittableVisualController
{
    private readonly HittableVisualController hittableVisualController = new();
    private Circuit tile;
    private CircuitVisuals visuals;
    public override T GetGameObject<T>() => tile as T;
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject){
        tile = (Circuit)dotsGameObject;
        visuals = dotsGameObject.GetComponent<CircuitVisuals>();
        hittableVisualController.Init(tile, visuals);
        base.Init(dotsGameObject);

    }

    public IEnumerator Hit(HitType hitType){
        if(tile.IsOn){
            visuals.spriteRenderer.sprite = visuals.OffSprite;

        }
        else{
            visuals.spriteRenderer.sprite = visuals.OnSprite;

        }
        yield return hittableVisualController.Hit(hitType);
    }

    public override void SetInitialColor()
    {
        
    }

    public IEnumerator Clear()
    {
        yield return hittableVisualController.Clear();
    }
}
