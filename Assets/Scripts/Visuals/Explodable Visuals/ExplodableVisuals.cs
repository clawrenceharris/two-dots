using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableVisuals : Visuals, IHittableVisuals
{
    public HittableVisuals hittableVisuals;
    public float ExplodeDuration = 0.7f;
    public float HitDuration => hittableVisuals.HitDuration;

    public float ClearDuration => hittableVisuals.ClearDuration;
}
