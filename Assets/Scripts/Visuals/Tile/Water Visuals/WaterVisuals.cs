using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVisuals : TileVisuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;
    public SpriteRenderer water;

    public float ClearDuration => hittableVisuals.ClearDuration;

    public float HitDuration => hittableVisuals.HitDuration;

    
}
