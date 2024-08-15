using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombVisuals : DotVisuals, IExplodableVisuals
{
    public ExplodableVisuals ExplodableVisuals;
    public SpriteRenderer[] BombSprites;
    public float ExplodeDuration => ExplodableVisuals.ExplodeDuration;
}
