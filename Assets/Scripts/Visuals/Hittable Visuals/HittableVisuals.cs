using UnityEngine;

[System.Serializable]

public class HittableVisuals : IHittableVisuals
{
    public static float defaultClearDuration = 0.5f;
    public static float hitDuration = 0.8f;
    [SerializeField] private float clearDuration;
    public float ClearDuration => clearDuration;
}