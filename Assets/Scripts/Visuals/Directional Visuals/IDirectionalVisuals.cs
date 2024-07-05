using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public interface IDirectionalVisuals
{
    float RotationDuration { get; }
    Ease RotationEase { get;  }
}
