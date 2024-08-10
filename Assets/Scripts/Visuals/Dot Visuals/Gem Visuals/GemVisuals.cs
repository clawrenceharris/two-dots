using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemVisuals : DotVisuals, IExplodableVisuals
{
    private float explodeDuration; 
    public float ExplodeDuration => explodeDuration;

    public SpriteRenderer GemTop;

    public SpriteRenderer GemBottom;
    public SpriteRenderer GemRight;
    public SpriteRenderer GemLeft;

    public SpriteRenderer GemTopLeft;

    public SpriteRenderer GemBottomLeft;

    public SpriteRenderer GemTopRight;

    public SpriteRenderer GemBottomRight;

}
