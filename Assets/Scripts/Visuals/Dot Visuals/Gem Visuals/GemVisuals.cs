using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemVisuals : DotVisuals, IExplodableVisuals
{
    public ExplodableVisuals explodableVisuals;
    public float ExplodeDuration => explodableVisuals.ExplodeDuration;

    public SpriteRenderer GemTop;

    public SpriteRenderer GemBottom;
    public SpriteRenderer GemRight;
    public SpriteRenderer GemLeft;

    public SpriteRenderer GemTopLeft;

    public SpriteRenderer GemBottomLeft;

    public SpriteRenderer GemTopRight;

    public SpriteRenderer GemBottomRight;

    public SpriteRenderer VerticalRay;
    public SpriteRenderer HorizontalRay;

}
