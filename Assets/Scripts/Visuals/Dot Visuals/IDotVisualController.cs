using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public interface IDotVisualController
{
    public Dot Dot { get; }
    public DotVisuals Visuals { get; }
    public SpriteRenderer SpriteRenderer { get; }
    public void Init(Dot dot);
    public IEnumerator PreviewHit(PreviewHitType hitType);
    public void SetColor(Color color);
    public void DisableSprites();
    public void ActivateBomb();
    public void EnableSprites();
    public IEnumerator Clear();
    public IEnumerator Hit(HitType hitType);
    public IEnumerator BombHit();

}
