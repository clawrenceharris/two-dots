using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MonsterDotVisuals : DotVisuals, INumerableVisuals, IDirectionalVisuals
{
    public NumerableVisuals numerableVisuals;

    public SpriteRenderer Digit1 => numerableVisuals.Digit1;

    public SpriteRenderer Digit2 => numerableVisuals.Digit2;


    public DirectionalVisuals directionalVisuals;

    public float RotationDuration =>directionalVisuals.RotationDuration;

    public Ease RotationEase => directionalVisuals.RotationEase;
}
