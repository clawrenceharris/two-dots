using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class CircuitVisualController : TileVisualController
{
    private Circuit tile;
    private CircuitVisuals visuals;
    public override T GetGameObject<T>() => tile as T;
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject){
        tile = (Circuit)dotsGameObject;
        visuals = dotsGameObject.GetComponent<CircuitVisuals>();
        base.Init(dotsGameObject);

    }

    protected override void SetUp()
    {
        base.SetUp();
        UpdateSprite();
    }

    
    
    public void UpdateSprite(){
        if(!tile.IsActive){
            visuals.OffSprite.enabled =true;
            visuals.OnSprite.enabled =false;
        }
        else{
            visuals.OffSprite.enabled =false;
            visuals.OnSprite.enabled =true;

        }    
    }


    public override void SetColor(Color color)
    {
        base.SetColor(color);
        visuals.OnSprite.color = color;
    }
   
    public override void SetInitialColor()
    {
        Color bgColor = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        visuals.spriteRenderer.color = bgColor;
        visuals.OnSprite.color = ColorUtils.LightenColor(bgColor,0.6f);;
    }

   
}
