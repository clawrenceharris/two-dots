using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableVisuals : Visuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;
    public float explodeDuration = 0.7f;
    public SpriteRenderer BombHitSprite => hittableVisuals.BombHitSprite;

    public float ClearDuration => hittableVisuals.ClearDuration;
}
