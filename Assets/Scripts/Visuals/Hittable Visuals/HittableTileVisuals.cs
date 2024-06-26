using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableTileVisuals : TileVisuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;
    public SpriteRenderer BombHitSprite => hittableVisuals.BombHitSprite;

    public float ClearDuration => hittableVisuals.ClearDuration;
}
