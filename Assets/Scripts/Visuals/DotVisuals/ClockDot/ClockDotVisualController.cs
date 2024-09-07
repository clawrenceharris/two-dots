using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using System;

public class ClockDotVisualController : BlankDotBaseVisualController, 
INumerableVisualController
{
    private ClockDot dot;
    private  ClockDotVisuals visuals;
    private readonly NumerableVisualController numerableVisualController = new();
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (ClockDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<ClockDotVisuals>();
        numerableVisualController.Init(dot, visuals.numerableVisuals);
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
        
         var settings = new AnimationSettings{
            Duration = moveDuration
        };
        foreach (Vector2Int pos in path)
        {
            if(pos == path.Last()){
                AnimationCurve  curve = new(
                    new Keyframe(0f, 0f, 0f, 0f),        // Start point
                    new Keyframe(0.3f, 0.7f, 0f, 0f),    // Peak with higher value for more elasticity
                    new Keyframe(0.7f, 0.3f, 0f, 0f),    // Bounce back to a lower value
                    new Keyframe(1f, 1f, 0f, 0f)         // End point
                );

                settings.Curve = curve;
                dot.StartCoroutine(Animate(new MoveAnimation{
                    Target = new Vector2(pos.x, pos.y) * Board.offset,
                    Settings = settings,
                    
                }));
            }
            else{
                AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
                settings.Curve = curve;
                dot.StartCoroutine(Animate(new MoveAnimation{
                    Target = new Vector2(pos.x, pos.y) * Board.offset,
                    Settings = settings
                }));
            }
            
            yield return new WaitForSeconds(moveDuration);
            onMoved?.Invoke();
        }


    }

    
    
    
    
    private void MoveClockDotPreviews(ConnectionArgs args){

        List<ConnectableDot> connectedDots = ConnectionManager.Connection.ConnectedDots.ToList();
        var settings = new AnimationSettings{
            Ease = Ease.OutElastic,
            Amplitude =0.5f,
            Period = 1.5f,
            Duration = 0.5f
        };

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

                }
                    clockDot.VisualController.Animate(new MoveAnimation{
                        Target = lastAvailableDot.transform.position,
                        Settings = settings
                    });
                
            }
        }
    }
    
    public IEnumerator ScaleNumbers()
    {
        yield return numerableVisualController.ScaleNumbers();
    }
}