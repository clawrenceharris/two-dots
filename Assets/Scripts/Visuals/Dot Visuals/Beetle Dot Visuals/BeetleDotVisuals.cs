using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BeetleDotVisuals : DotVisuals
{

    public GameObject leftWingLayer1;
    public GameObject leftWingLayer2;
    public GameObject leftWingLayer3;
    public GameObject rightWingLayer1;
    public GameObject rightWingLayer2;
    public GameObject rightWingLayer3;
    public GameObject rightWhiteWingLayer1;
    public GameObject rightWhiteWingLayer2;
    public GameObject rightWhiteWingLayer3;
    public GameObject leftWhiteWingLayer1;
    public GameObject leftWhiteWingLayer2;
    public GameObject leftWhiteWingLayer3;
    public Transform rightWings;
    public Transform leftWings;
    public Ease rotationEase = Ease.OutCubic;
    public float rotationSpeed = 0.2f;
    
}
