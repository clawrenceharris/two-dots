using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using System;

public class ClockDotVisualController : BlankDotBaseVisualController, 
INumerableVisualController,
IPreviewableVisualController
{
    private ClockDot dot;
    private  ClockDotVisuals visuals;
    private readonly PreviewableVisualController previewableVisualController = new();
    private readonly NumerableVisualController numerableVisualController = new();
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (ClockDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<ClockDotVisuals>();
        numerableVisualController.Init(dot, visuals.numerableVisuals);
        previewableVisualController.Init(this);
        base.Init(dotsGameObject);

        
    }

    public override void Unsubscribe(){
        ConnectionManager.onDotConnected -= MoveClockDotPreviews;
        ConnectionManager.onDotDisconnected -= MoveClockDotPreviews;
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
        base.Unsubscribe();
    }
    public override void Subscribe(){
        ConnectionManager.onDotConnected += MoveClockDotPreviews;
        ConnectionManager.onDotDisconnected += MoveClockDotPreviews;
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        base.Subscribe();
    }



    private void OnConnectionEnded(LinkedList<ConnectableDot> connectedDots){
        if(!connectedDots.Contains(dot)){
            return;
        }
        visuals.clockDotPreview.SetActive(false);
        visuals.clockDotPreview.transform.SetParent(dot.transform);
        visuals.clockDotPreview.transform.position = dot.transform.position;
    }
    protected override void SetUp()
    {
        UpdateNumbers(dot.CurrentNumber);
        base.SetUp();
    }


    public override void SetInitialColor()
    {
        visuals.top.color = ColorSchemeManager.CurrentColorScheme.clockDot;
        visuals.middle.color = new Color(255, 255, 255, 0.6f);
        visuals.shadow.color = new Color(255, 255, 255, 0.6f);
        base.SetInitialColor();
    }



    public override void SetColor(Color color)
    {

        base.SetColor(color);

        foreach (Transform child in dot.transform)
        {
            if (child.TryGetComponent(out SpriteRenderer sr))
            {
                //change to bomb color and keep the original alpha value
                sr.color = new Color(color.r, color.g, color.b, sr.color.a);
            }
        }

       
    }



    public void UpdateNumbers(int number)
    {
        numerableVisualController.UpdateNumbers(number);

    }

    public IEnumerator DoMove(List<Vector2Int> path, Action onMoved)
    {
        float duration = ClockDotVisuals.moveDuration;
        float moveDuration = duration / path.Count;
        
        CoroutineHandler.StartStaticCoroutine(Move(path.Select(pos=> new Vector3(pos.x, pos.y)).ToList()));
       
        yield return new WaitForSeconds(moveDuration);
        


    }

    
    
    
    
    private void MoveClockDotPreviews(ConnectionArgs args){

        List<ConnectableDot> connectedDots = ConnectionManager.Connection.ConnectedDots.ToList();
        

        if (ConnectionManager.ConnectedDots.Contains(dot))
        {

            visuals.clockDotPreview.SetActive(true);
            visuals.clockDotPreview.transform.SetParent(null);
            
        }
        int count = 0;
        ConnectableDot lastAvailableDot;
        
        for (int i = connectedDots.Count - 1; i >= 0; i--)
        {
            Dot dot = connectedDots[i];
            if (dot is ClockDot clockDot)
            {

                count++;

                lastAvailableDot = connectedDots[^count];
                if(lastAvailableDot.DotType.IsBasicDot() || lastAvailableDot.DotType.IsClockDot()){
                    clockDot.VisualController.Move(lastAvailableDot.transform.position);
                }
                
            }
        }
    }
    
    public IEnumerator ScaleNumbers()
    {
        yield return numerableVisualController.ScaleNumbers();
    }

    public IEnumerator PreviewClear()
    {
        yield return previewableVisualController.PreviewClear();
    }

    public IEnumerator PreviewHit()
    {
        yield return previewableVisualController.PreviewHit();
    }

    public IEnumerator Idle()
    {
        yield return previewableVisualController.Idle();
    }
}