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

    protected override void SetUp()
    {
        base.SetUp();
        SetSprite();
    }

    
    
    private void SetSprite(){
        if(!tile.IsOn){
            visuals.OffSprite.enabled =true;
            visuals.OnSprite.enabled =false;
        }
        else{
            visuals.OffSprite.enabled =false;
            visuals.OnSprite.enabled =true;

        }    
    }
    

    
    public IEnumerator Hit(HitType hitType){
        SetSprite();
        yield return hittableVisualController.Hit(hitType);
    }

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        visuals.OnSprite.color = new Color(255,255, 255, 0.8f);
    }

    public IEnumerator Clear()
    {
        yield return hittableVisualController.Clear();
    }
}
