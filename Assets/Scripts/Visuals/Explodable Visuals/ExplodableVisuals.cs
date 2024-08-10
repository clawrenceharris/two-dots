using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExplodableVisuals : IExplodableVisuals
{
    public HittableVisuals hittableVisuals;
    [SerializeField]private float explodeDuration;

    public float ExplodeDuration => explodeDuration;
}
