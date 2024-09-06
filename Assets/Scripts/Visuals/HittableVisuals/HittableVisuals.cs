using UnityEngine;

[System.Serializable]

public class HittableVisuals : IHittableVisuals
{
    public const float defaultClearDuration = 0.4f;
    [SerializeField] private float hitDuration;
    [SerializeField] private float clearDuration;

    public float ClearDuration => clearDuration;
    public float HitDuration => hitDuration;

}