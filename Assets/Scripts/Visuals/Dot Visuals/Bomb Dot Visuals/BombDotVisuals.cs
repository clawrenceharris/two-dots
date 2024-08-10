using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDotVisuals : DotVisuals, IExplodableVisuals
{
    public ExplodableVisuals ExplodableVisuals;
    public SpriteRenderer[] BombSprites;
    public float ExplodeDuration => ExplodableVisuals.ExplodeDuration;
}
