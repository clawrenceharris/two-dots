using System;
using DG.Tweening;
using UnityEngine;

public interface IShakeSettings : IAnimationSettings{
    public float Randomness {get; set;}
    public bool Snapping {get; set;}
    public bool FadeOut {get; set;}
    public ShakeRandomnessMode ShakeRandomnessMode {get; set;}
    public int Vibrato {get; set;}
    public Vector2 Strength {get; set;}


}

[Serializable]
public class ShakeSettings : AnimationSettings, IShakeSettings
{

    [SerializeField]private float randomness = 90f;
    [SerializeField]private bool snapping = false;
    [SerializeField]private bool fadeOut = false;
    [SerializeField]private ShakeRandomnessMode shakeRandomnessMode = ShakeRandomnessMode.Harmonic;
    [SerializeField]private int vibrato = 10;
    [SerializeField]private Vector2 strength = new(0.02f, 0.02f);
    public float Randomness {get => randomness; set => randomness = value;}
    public bool Snapping {get => snapping; set => snapping = value;}
    public bool FadeOut {get => fadeOut; set => fadeOut = value;}
    public ShakeRandomnessMode ShakeRandomnessMode {get => shakeRandomnessMode; set => shakeRandomnessMode = value;}
    public int Vibrato {get => vibrato; set => vibrato = value;}
    public Vector2 Strength {get => strength; set => strength = value;}

    


    
}