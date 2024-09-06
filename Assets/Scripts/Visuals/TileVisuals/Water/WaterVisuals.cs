using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVisuals : TileVisuals, IHittableVisuals
{
    public HittableVisuals HittableVisuals;
    public SpriteRenderer Water;
    public ConnectorLine WaterExtension;
    public SpriteRenderer WaterEndSprite;
    public float ClearDuration => HittableVisuals.ClearDuration;

    public float HitDuration => HittableVisuals.HitDuration;

    [Header("Water Sprites")]
    public Sprite TopRightCornerSprite;
    public Sprite TopLeftCornerSprite;
    public Sprite BottomRightCornerSprite;
    public Sprite BottomLeftCornerSprite;
   
    public Sprite HorizontalEdgeSprite;
    public Sprite VerticalEdgeSprite;
    public Sprite RightEdgeSprite;
    public Sprite LeftEdgeSprite;
    public Sprite TopEdgeSprite;
    public Sprite BottomEdgeSprite;

    public Sprite RightEndSprite;
    public Sprite LeftEndSprite;
    public Sprite TopEndSprite;
    public Sprite BottomEndSprite;

    public Sprite FullSprite;
    public Sprite IsolatedSprite;
    public Sprite RightEndWaterSprite;
    public Sprite LeftEndWaterSprite;
    public Sprite TopEndWaterSprite;
    public Sprite BottomEndWaterSprite;

    public Sprite FullWaterSprite;
    public Sprite IsolatedWaterSprite;
    public Sprite TopRightWaterSprite;
    public Sprite TopLeftWaterSprite;
    public Sprite BottomRightWaterSprite;
    public Sprite BottomLeftWaterSprite;
    public Sprite TempWaterSprite;
}
