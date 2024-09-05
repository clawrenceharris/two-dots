using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class GemVisualController : ColorableDotVisualController
{
    private GemVisuals Visuals => GetVisuals<GemVisuals>();
    private Gem Dot => GetGameObject<Gem>();
    


    public override void Init(DotsGameObject dotsGameObject)
    {
        Subscribe();
        base.Init(dotsGameObject);

    }

    protected override void SetUp()
    {
        InitVerticalRay(GetVerticalRay());
        InitHorizontalRay(GetHorizontalRay());
        base.SetUp();
    }

    protected void InitVerticalRay(SpriteRenderer ray){
        if(ray == null){
            return;
        }
        Camera camera = Camera.main;
        float initialScaleX = ray.transform.localScale.x;
        float height = camera.orthographicSize * 2;
        ray.transform.localScale = new Vector2(initialScaleX, height * 2);
        ray.color = ColorSchemeManager.FromDotColor(Dot.Color);
    }
    protected void InitHorizontalRay(SpriteRenderer ray){
        if(ray == null){
            return;
        }
        Camera camera = Camera.main;
        float initialScaleX = ray.transform.localScale.x;
        float width = camera.orthographicSize * 2 * camera.aspect;
        ray.transform.localScale = new Vector2(initialScaleX, width * 2);
        ray.color = ColorSchemeManager.FromDotColor(Dot.Color);
    }
    public override void SetInitialColor()
    {
        base.SetInitialColor();
        SetColor(ColorSchemeManager.FromDotColor(Dot.Color));
    }

    public override void SetColor(Color color)
    {
        Visuals.spriteRenderer.color = ColorUtils.LightenColor(color, 0.1f);;
        Visuals.GemTopLeft.color = color;
        Visuals.GemBottomRight.color = color;
        Visuals.GemBottomLeft.color = ColorUtils.DarkenColor(color, 0.4f);
        Visuals.GemLeft.color = ColorUtils.DarkenColor(color, 0.3f);
        Visuals.GemTop.color = ColorUtils.LightenColor(color, 0.48f);
        Visuals.GemRight.color = ColorUtils.LightenColor(color, 0.48f);
        Visuals.GemTopRight.color = ColorUtils.LightenColor(color, 0.7f);
        Visuals.GemBottom.color = ColorUtils.DarkenColor(color, 0.3f);
        
    }
    

    public abstract SpriteRenderer GetHorizontalRay();
    public abstract SpriteRenderer GetVerticalRay();
    
    public void ShowRays(){
        ShowHorizontalRay();
        ShowVerticalRay();
    
       
    }
    private void ShowHorizontalRay(){
        SpriteRenderer ray = GetHorizontalRay();
        
        if(ray == null){
            return;
        }
        ray.enabled = true;     
        ray.transform.SetParent(null);

        ray.transform.position = new Vector2(Board.Width / 2 * Board.offset, Dot.transform.position.y);

        
        
        
    }
    private void ShowVerticalRay(){
        SpriteRenderer ray = GetVerticalRay();
        
        if(ray == null){
            return;
        }
        ray.enabled = true;     
        ray.transform.SetParent(null);

        ray.transform.position = new Vector2(ray.transform.position.x, Board.Height / 2 * Board.offset );

        
        
        
    }

    private void DestroyRays(){
        SpriteRenderer hRay = GetHorizontalRay();
        SpriteRenderer vRay = GetVerticalRay();

        if(hRay){
            Object.Destroy(hRay);
        }
        if(vRay){
            Object.Destroy(vRay);  
        }
    }
   
    private void HideRays(){
        SpriteRenderer vRay = GetVerticalRay();
        SpriteRenderer hRay = GetHorizontalRay();
        if(vRay){
            vRay.enabled = false;
        }
        if(hRay){
            hRay.enabled = false;
        }
            
        
        
    }
       protected void OnConnectionEnded(LinkedList<ConnectableDot> dots){
            if(Dot.WasHit)
                DestroyRays();
            
            else
                HideRays();
        
    }

    protected void OnSquareMade(Square square){
        if(Dot.HitRule.Validate(Dot, null)){
            SetColor(ColorUtils.LightenColor(ColorSchemeManager.FromDotColor(Dot.Color), 0.2f));
            ShowRays();
        }
    }

    public abstract void Subscribe();
    public abstract void Unsubscribe();
    protected void OnDotDisconnected(ConnectionArgs args){
        
        HideRays();
        
    }

    public IEnumerator Explode(){
       
        //yield return new WaitForSeconds(Visuals.ClearDuration);
        yield break;

    }
    
}
