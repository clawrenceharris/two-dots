using UnityEngine;


[System.Serializable]

public class NumerableVisuals : INumerableVisuals
{
    [SerializeField] private SpriteRenderer digit1;
    [SerializeField] private SpriteRenderer digit2;
    public SpriteRenderer Digit1 => digit1;

    public SpriteRenderer Digit2 => digit2;
}