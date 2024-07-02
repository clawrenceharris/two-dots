using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotVisuals : Visuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;

    public SpriteRenderer outerDot;
    public static float outerDotScaleDuration = 0.8f;
    public static float outerDotFadeDuration = 0.8f;
    public SpriteRenderer BombHitSprite => hittableVisuals.BombHitSprite;

    public float ClearDuration => hittableVisuals.ClearDuration;




}
