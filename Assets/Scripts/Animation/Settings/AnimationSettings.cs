using DG.Tweening;
using UnityEngine;

public class AnimationSettings{
    public float Duration { get; set; } = 1f;
    public Ease Ease { get; set; }
    public AnimationCurve Curve { get; set; }
    public float Amplitude { get; set; } = 0.1f;
    public float Period { get; set; } = 0.5f;

    public int Loops { get; set; } = 1;

}