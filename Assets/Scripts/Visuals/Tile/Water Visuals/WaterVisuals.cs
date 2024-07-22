using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVisuals : TileVisuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;
    public SpriteRenderer water;

    public float ClearDuration => hittableVisuals.ClearDuration;

    public float HitDuration => hittableVisuals.HitDuration;

    [Header("Water Sprites")]
    public Sprite topRightCorner;
    public Sprite topLeftCorner;
    public Sprite bottomRightCorner;
    public Sprite bottomLeftCorner;
   
    public Sprite horizontalEdge;
    public Sprite verticalEdge;
    public Sprite rightEdge;
    public Sprite leftEdge;
    public Sprite topEdge;
    public Sprite bottomEdge;

    public Sprite rightEnd;
    public Sprite leftEnd;
    public Sprite topEnd;
    public Sprite bottomEnd;

    public Sprite full;

    public Sprite waterRight;
    public Sprite waterLeft;
    public Sprite waterTop;
    public Sprite waterBottom;

    public Sprite waterFull;

    
}
