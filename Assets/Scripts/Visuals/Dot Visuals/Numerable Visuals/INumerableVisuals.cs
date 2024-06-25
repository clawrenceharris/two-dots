using UnityEngine;

public interface INumerableVisuals : IVisuals
{
    public SpriteRenderer Digit1 { get; }
    public SpriteRenderer Digit2 { get; }
}