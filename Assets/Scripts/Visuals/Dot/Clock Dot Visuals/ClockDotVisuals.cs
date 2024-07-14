using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockDotVisuals : BlankDotVisuals, INumerableVisuals
{
    public NumerableVisuals numerableVisuals;

    public SpriteRenderer top;
    public SpriteRenderer middle;
    public SpriteRenderer shadow;
    public GameObject clockDotPreview;
    internal static float moveDuration = 0.5f;

    public SpriteRenderer Digit1 => numerableVisuals.Digit1;

    public SpriteRenderer Digit2 => numerableVisuals.Digit2;

}
