using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BeetleDotVisuals : DotVisuals
{
    public GameObject rightWingLayer3;
    public GameObject leftWingLayer3;
    public GameObject rightWingLayer1;
    public GameObject leftWingLayer1;
    public GameObject leftWingLayer2;
    public GameObject rightWingLayer2;
    public Transform rightWings;
    public Transform leftWings;
    public Transform leftPivot;
    public Transform rightPivot;
    public Ease rotationEase = Ease.OutCubic;
    public float rotationSpeed = 0.2f;
    public float moveSpeed = 0.5f;
    public float clearAmplitude = 1.2f;
    public float clearFrequeuncy = 2f;
    public float clearSpeed = 10f;
}
