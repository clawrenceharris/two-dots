using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDotVisuals : ExplodableVisuals
{
    public SpriteRenderer[] bombSprites;
    public GameObject line;
    public static float bombHitDuration = 0.1f;
    public Sprite lineSprite;
}
