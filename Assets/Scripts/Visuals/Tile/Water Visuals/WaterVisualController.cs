using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVisualController : TileVisualController
{
    private Water tile;
    private WaterVisuals visuals;
    public override T GetGameObject<T>() => tile as T;


    public override T GetVisuals<T>() => visuals as T;
   

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Water)dotsGameObject;
        visuals = dotsGameObject.GetComponent<WaterVisuals>();
        base.Init(dotsGameObject);

    }

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = Color.white;
        visuals.water.color = ColorSchemeManager.CurrentColorScheme.water;
    }

    public override void SetColor(Color color)
    {
        base.SetColor(color);
        visuals.water.color = color;

    }


}
