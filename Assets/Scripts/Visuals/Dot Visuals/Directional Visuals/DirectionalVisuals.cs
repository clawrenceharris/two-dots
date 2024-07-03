using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DirectionalVisuals : IDirectionalVisuals
{
    [SerializeField] private float rotationDuration;
    [SerializeField] private Ease rotationEase;
    public float RotationDuration => rotationDuration;

    public Ease RotationEase => rotationEase;
}
