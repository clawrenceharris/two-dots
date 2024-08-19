using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleGemVisualController : GemVisualController, IDirectionalVisualController
{
    private RectangleGem dot;
    private GemVisuals visuals;
    private readonly DirectionalVisualController directionalVisualController = new();
    
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    
    public IEnumerator DoRotateAnimation()
    {
        //do nothing; no rotation animation needed
        yield break;
    }
    
   

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (RectangleGem)dotsGameObject;
        visuals = dotsGameObject.GetComponent<GemVisuals>();
        directionalVisualController.Init(dot, null);
       
        base.Init(dotsGameObject);

    }
    protected override void SetUp()
    {
        //rotate the gem to match the initial direction
        UpdateRotation();
        base.SetUp();
    }
   
    private void FlipSpritesHorizontally(bool flipX){
        foreach(SpriteRenderer spriteRenderer in visuals.Sprites){
            
            spriteRenderer.flipY = flipX;
        }
    }
    private void FlipSpritesVertically(bool flipY){
        foreach(SpriteRenderer spriteRenderer in visuals.Sprites){
            spriteRenderer.flipX = flipY;
        }
    }
   
    
    public void UpdateRotation()
    {
        directionalVisualController.UpdateRotation();
        FlipSpritesHorizontally(false);
        FlipSpritesVertically(false);
        float angle = dot.GetRotation().z;
        // Determine which flips to apply based on the angle
        if (angle == 90f || angle == 270f)
        {
            FlipSpritesHorizontally(true);
        }
        else if (angle == 180f)
        {
            FlipSpritesHorizontally(true);
            FlipSpritesVertically(true);
        }
        
    }

    public override void Subscribe()
    {
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        Connection.onSquareMade += OnSquareMade;
        Board.onBoardCreated += OnBoardCreated;

    }

    public override void Unsubscribe()
    {
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
        ConnectionManager.onDotDisconnected -= OnDotDisconnected;
        Connection.onSquareMade -= OnSquareMade;
        Board.onBoardCreated -= OnBoardCreated;
    }

    public override SpriteRenderer GetHorizontalRay()
    {
        float angle = dot.GetRotation().z;
        if (angle == 90f || angle == 270f)
        {
            return visuals.HorizontalRay;
        }
       
        return null;
    }

    public override SpriteRenderer GetVerticalRay()
    {
        float angle = dot.GetRotation().z;
        if (angle == 0f  || angle == 180 || angle == 360f)
        {
            return visuals.VerticalRay;
        }
        return null;
    }
}

