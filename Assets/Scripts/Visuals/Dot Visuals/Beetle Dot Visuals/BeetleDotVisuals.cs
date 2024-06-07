using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BeetleDotVisuals : DotVisuals
{
    public GameObject rightWing3;
    public GameObject leftWing3;
    public GameObject rightWing1;
    public GameObject leftWing1;
    public GameObject leftWing2;
    public GameObject rightWing2;
    public Transform rightWings;
    public Transform leftWings;
    public Transform leftPivot;
    public Transform rightPivot;
    public Ease rotationEase = Ease.OutCubic;
    public float rotationSpeed = 0.15f;
    public float moveSpeed = 0.5f;



}
