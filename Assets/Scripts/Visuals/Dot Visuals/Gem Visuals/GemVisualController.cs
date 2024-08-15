using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GemVisualController : ColorableDotVisualController, IPreviewableVisualController
{
    private Gem dot;
    private static Board board;
    private GemVisuals visuals;

    public override T GetGameObject<T>() => dot as T;
   
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (Gem)dotsGameObject;
        visuals = dotsGameObject.GetComponent<GemVisuals>();   
        base.Init(dotsGameObject);

        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        Connection.onSquareMade += OnSquareMade;
        Board.onBoardCreated += OnBoardCreated;
    }
       
    private void OnBoardCreated(Board board){
        GemVisualController.board = board;
    } 
    
    protected override void SetUp()
    {
        base.SetUp();
        Camera camera = Camera.main;
        float initialScaleX = visuals.HorizontalRay.transform.localScale.x;
        float width = camera.orthographicSize * 2 * camera.aspect;
        float height = camera.orthographicSize * 2;
        visuals.HorizontalRay.transform.localScale = new Vector2(initialScaleX, width * 2);
        visuals.VerticalRay.transform.localScale = new Vector2(initialScaleX, height * 2);
        
    }

    public override void SetInitialColor()
    {
        base.SetInitialColor();
        SetColor(ColorSchemeManager.FromDotColor(dot.Color));
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
    public IEnumerator DoClearPreviewAnimation()
    {
        yield return DoPreviewAnimation();
    }
    private IEnumerator DoPreviewAnimation(){
        float duration = 0.8f;
        float strength = 0.08f; 
        int vibrato = 10; //number of shakes
        float randomness = 2; 
        dot.transform.DOShakePosition(duration, new Vector3(strength,strength, 0), vibrato, randomness, false, true);

        yield return new WaitForSeconds(duration /2);
    }
    public IEnumerator DoHitPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoIdleAnimation()
    {
        yield break;
    }



    public override IEnumerator Hit(HitType hitType)
    {
        SetColor(ColorUtils.LightenColor(ColorSchemeManager.FromDotColor(dot.Color), 0.2f));
        ShowRays();

        yield return base.Hit(hitType);

    }

    public void ShowRays(){

        visuals.HorizontalRay.enabled = true;     
        visuals.VerticalRay.enabled = true;     
        visuals.HorizontalRay.transform.SetParent(null);
        visuals.VerticalRay.transform.SetParent(null);

        visuals.HorizontalRay.transform.position = new Vector2(board.Width / 2 * Board.offset, visuals.HorizontalRay.transform.position.y);
        visuals.VerticalRay.transform.position = new Vector2(visuals.VerticalRay.transform.position.x, board.Height / 2 * Board.offset);

        
    }

    public override IEnumerator Clear(float duration)
    {
        Object.Destroy(visuals.HorizontalRay);
        Object.Destroy(visuals.VerticalRay);
        return base.Clear(duration);
    }
    public void HideRays(){
        
        visuals.HorizontalRay.enabled = false;     
        visuals.VerticalRay.enabled = false;
    }
   
    private void OnConnectionEnded(LinkedList<ConnectableDot> dots){
        if(ConnectionManager.ToHitBySquare.Contains(dot)){
            HideRays();
        }
    }

    private void OnSquareMade(Square square){
        if(dot.HitRule.Validate(dot, null)){
            SetColor(ColorUtils.LightenColor(ColorSchemeManager.FromDotColor(dot.Color), 0.2f));
            ShowRays();
        }
    }

    public void Unsubscribe(){
       
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
        ConnectionManager.onDotDisconnected -= OnDotDisconnected;
        Connection.onSquareMade -= OnSquareMade;

    }
    private void OnDotDisconnected(ConnectableDot dot){
        
        HideRays();
        
    }

    public IEnumerator Explode(){
       
        yield return new WaitForSeconds(visuals.ClearDuration);
        

    }
    
}
