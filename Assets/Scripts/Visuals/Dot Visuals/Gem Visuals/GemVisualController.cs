using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemVisualController : ColorableDotVisualController, IPreviewableVisualController
{
    private Gem dot;

    private GemVisuals visuals;

    public IEnumerator DoClearPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoHitPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoIdleAnimation()
    {
        yield break;
    }

    public override T GetGameObject<T>() => dot as T;
   
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (Gem)dotsGameObject;
        visuals = dotsGameObject.GetComponent<GemVisuals>();   
        color = ColorSchemeManager.FromDotColor(dot.Color);
        base.Init(dotsGameObject);

        ConnectionManager.onConnectionEnded += OnConnectionEnded;
    }

    protected override void SetUp()
    {
        base.SetUp();
        
        float initialScaleX = visuals.HorizontalRay.transform.localScale.x;
        float spriteWidth = visuals.HorizontalRay.sprite.bounds.size.x;
        float yScaleFactor = Screen.width / spriteWidth;
        visuals.HorizontalRay.transform.localScale = new Vector2(initialScaleX, yScaleFactor);
        visuals.VerticalRay.transform.localScale = new Vector2(initialScaleX, yScaleFactor);
    }
    public override void SetInitialColor()
    {
        base.SetInitialColor();
        SetColor(color);
    }

    public override void SetColor(Color color)
    {
        visuals.spriteRenderer.color = ColorUtils.LightenColor(color, 0.1f);;
        visuals.GemTopLeft.color = color;
        visuals.GemBottomRight.color = color;
        visuals.GemBottomLeft.color = ColorUtils.DarkenColor(color, 0.4f);
        visuals.GemLeft.color = ColorUtils.DarkenColor(color, 0.3f);
        visuals.GemTop.color = ColorUtils.LightenColor(color, 0.48f);
        visuals.GemRight.color = ColorUtils.LightenColor(color, 0.48f);
        visuals.GemTopRight.color = ColorUtils.LightenColor(color, 0.7f);
        visuals.GemBottom.color = ColorUtils.DarkenColor(color, 0.3f);
        visuals.HorizontalRay.color = color;
        visuals.VerticalRay.color = color;
    }
    private void ShowRays(){

        visuals.HorizontalRay.enabled = true;     
        visuals.VerticalRay.enabled = true;     

        
    }
    private void HideRays(){
        
        visuals.HorizontalRay.enabled = false;     
        visuals.VerticalRay.enabled = false;
    }
    public override IEnumerator Clear(float duration)
    {
        HideRays();

        return base.Clear(duration);
    }
    private void OnConnectionEnded(LinkedList<ConnectableDot> dots){
        HideRays();
    }

    public void Explode(){
        ShowRays();
    }
    
}
