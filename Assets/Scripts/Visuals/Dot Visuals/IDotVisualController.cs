using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDotVisualController
{
    public Dot Dot { get; }
    public DotVisuals Visuals { get; }
    public SpriteRenderer SpriteRenderer { get; }
    public void Init(Dot dot);
    public void ActivateBomb();
    public void DeactivateBomb();
    public void SetUp();
    public void SetColor();
    public void SetColor(Color color);
    public IEnumerator Clear();
    IEnumerator BombHit();
}
