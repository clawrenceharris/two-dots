using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceVisuals : Visuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;
    public SpriteRenderer cracks;
    public Sprite cracksHit1;
    public Sprite cracksHit2;
    public float HitDuration => hittableVisuals.HitDuration;

    public float ClearDuration => hittableVisuals.ClearDuration;
}
