using DG.Tweening;
using UnityEngine;

[System.Serializable]

public class HittableVisuals : IHittableVisuals
{
    public const float CLEAR_DURATION = 0.4f;
    public const Ease CLEAR_EASE = Ease.Linear;
    [SerializeField] private float hitDuration;
    [SerializeField] private float clearDuration;

    public float ClearDuration => clearDuration;
    public float HitDuration => hitDuration;

}