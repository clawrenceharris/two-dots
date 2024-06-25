using UnityEngine;

public class NumerableDotVisuals : DotVisuals, INumerableVisuals
{
    public SpriteRenderer digit1;
    public SpriteRenderer digit2;
    public Sprite[] numbers;
    public SpriteRenderer Digit1 => digit1;

    public SpriteRenderer Digit2 => digit2;

    public Sprite[] Numbers => numbers;
}