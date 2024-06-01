using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public interface ITileVisualController
{
    public void Init(Tile tile);
    public Tile Tile { get; }
    public TileVisuals Visuals { get; }
    public SpriteRenderer SpriteRenderer { get; }
    public IEnumerator Hit(HitType hitType);
    public IEnumerator BombHit();

    public IEnumerator Clear();
}
