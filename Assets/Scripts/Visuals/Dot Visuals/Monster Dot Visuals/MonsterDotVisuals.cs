using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDotVisuals : DotVisuals, INumerableVisuals
{
    public NumerableVisuals numerableVisuals;

    public SpriteRenderer Digit1 => numerableVisuals.Digit1;

    public SpriteRenderer Digit2 => numerableVisuals.Digit2;
}
