using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockDotVisuals : BlankDotVisuals, INumerableVisuals
{


    public SpriteRenderer top;
    public SpriteRenderer digit1;
    public SpriteRenderer digit2;
    public SpriteRenderer middle;
    public SpriteRenderer shadow;
    public GameObject clockDotPreview;

    public SpriteRenderer Digit1 => digit1;

    public SpriteRenderer Digit2 => digit2;

}
