using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittableVisuals
{
    SpriteRenderer BombHitSprite { get; }
    float ClearDuration { get; }
   
}
