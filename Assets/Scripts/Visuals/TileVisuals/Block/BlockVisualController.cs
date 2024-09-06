using System.Collections;
using UnityEngine;

public class BlockVisualController : TileVisualController
{
    private Block tile;
    private HittableTileVisuals visuals;

    public override T GetGameObject<T>() => tile as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Block)dotsGameObject;
        visuals = dotsGameObject.GetComponent<HittableTileVisuals>();
        base.Init(dotsGameObject);

    }

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
    }

   
    
}