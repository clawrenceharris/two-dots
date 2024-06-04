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
    public void SetUp();
    public IEnumerator PreviewHit(HitType hitType);
    public void SetColor();
    public void SetColor(Color color);
    public IEnumerator Clear();
    public IEnumerator Hit(HitType hitType);
    public IEnumerator BombHit();

}
