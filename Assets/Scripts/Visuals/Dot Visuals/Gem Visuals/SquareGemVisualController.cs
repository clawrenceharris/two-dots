using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGemVisualController : GemVisualController
{
    private SquareGem dot;
    private GemVisuals visuals;
    
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (SquareGem)dotsGameObject;
        visuals = dotsGameObject.GetComponent<GemVisuals>();
        base.Init(dotsGameObject);
    }
    
    public override void Subscribe()
    {
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        Connection.onSquareMade += OnSquareMade;
    }

    public override void Unsubscribe()
    {
       
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
        ConnectionManager.onDotDisconnected -= OnDotDisconnected;
        Connection.onSquareMade -= OnSquareMade;
    }

    public override SpriteRenderer GetHorizontalRay()
    {
        return visuals.HorizontalRay;
    }

    public override SpriteRenderer GetVerticalRay()
    {
        return visuals.VerticalRay;
    }
}
