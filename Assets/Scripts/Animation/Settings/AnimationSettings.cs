using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class AnimationSettings : IAnimationSettings{
    [SerializeField]private float duration;
    [SerializeField]private float amplitude;
    [SerializeField]private float period;
    [SerializeField]private Ease ease;
    [SerializeField]private int loops = 1;
    [SerializeField]private AnimationCurve curve;

    public AnimationCurve Curve {get => curve; set => curve = value;}
    public float Amplitude {get => amplitude; set => amplitude = value;}


    public float Period {get => period; set => period = value;}
    public int Loops {get => loops; set => loops = value;}
    public float Duration {get => duration; set => duration = value;}
    
    public Ease Ease {get => ease; set => ease = value;}

}