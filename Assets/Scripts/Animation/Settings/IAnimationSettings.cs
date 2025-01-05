using System;
using DG.Tweening;
using UnityEngine;

public interface IAnimationSettings{
    float Duration {get; set;}
    Ease Ease  {get; set;}
    AnimationCurve Curve  {get; set;}
    float Amplitude  {get; set;}
    float Period  {get; set;}

    int Loops  {get; set;}

}