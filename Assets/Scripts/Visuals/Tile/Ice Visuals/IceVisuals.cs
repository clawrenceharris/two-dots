using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceVisuals : Visuals , IHittableVisuals
{
    public HittableVisuals hittableVisuals;


    public float ClearDuration => hittableVisuals.ClearDuration;
}
