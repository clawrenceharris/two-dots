using UnityEngine;

[System.Serializable]

public class HittableVisuals : IHittableVisuals
{
    public static float defaultClearDuration = 0.5f;
    public static float hitDuration = 0.8f;
    public static float bombHitDuration = 0.2f;
    [SerializeField] private float clearDuration;
    [SerializeField] private SpriteRenderer bombHitSprite;

    public SpriteRenderer BombHitSprite => bombHitSprite;
    public float ClearDuration => clearDuration;
}