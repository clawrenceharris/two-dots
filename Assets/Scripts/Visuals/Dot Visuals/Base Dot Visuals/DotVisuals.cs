using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotVisuals : Visuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;

    public SpriteRenderer outerDot;
    public static float selectionAnimationDuration = 0.6f;
    public Sprite bombHitSprite;
    public float ClearDuration => hittableVisuals.ClearDuration;
    public float HitDuration => hittableVisuals.HitDuration;

    public static float DefaultDotSize;




}
