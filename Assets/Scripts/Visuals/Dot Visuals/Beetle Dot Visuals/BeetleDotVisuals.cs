using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeetleDotVisuals : DotVisuals, IDirectionalVisuals
{
    public GameObject leftWingLayer1;
    public GameObject leftWingLayer2;
    public GameObject leftWingLayer3;
    public GameObject rightWingLayer1;
    public GameObject rightWingLayer2;
    public GameObject rightWingLayer3;
    public SpriteRenderer[] sprites;
    public Transform rightWings;
    public Transform leftWings;
    public Ease rotationEase = Ease.OutCubic;
    public float rotationSpeed = 0.2f;
    public static float moveDuration = 0.5f;

    public DirectionalVisuals directionalVisuals;

    public float RotationDuration => directionalVisuals.RotationDuration;

    public Ease RotationEase => directionalVisuals.RotationEase;

}
