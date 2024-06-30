using UnityEngine;

[System.Serializable]
public class HittableVisuals : Visuals, IHittableVisuals
{
    public static float defaultClearDuration = 0.5f;
    public static float hitDuration = 0.5f;

    public float clearDuration;
    public SpriteRenderer bombHitSprite;

    public SpriteRenderer BombHitSprite => bombHitSprite;
    public float ClearDuration => clearDuration;
    
}