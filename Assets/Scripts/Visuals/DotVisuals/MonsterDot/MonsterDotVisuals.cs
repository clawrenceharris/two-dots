using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MonsterDotVisuals : DotVisuals, INumerableVisuals, IDirectionalVisuals
{
    public NumerableVisuals NumerableVisuals;
    public SpriteRenderer Digit1 => NumerableVisuals.Digit1;

    public SpriteRenderer Digit2 => NumerableVisuals.Digit2;


    public SpriteRenderer EyeLids;

    public DirectionalVisuals DirectionalVisuals;
    public static float MoveDuration = 0.6f;
    public SpriteRenderer LeftEye;
    public SpriteRenderer RightEye;
    public SpriteRenderer[] AllSprites;

    public float RotationDuration => DirectionalVisuals.RotationDuration;

    public Ease RotationEase => DirectionalVisuals.RotationEase;
}
