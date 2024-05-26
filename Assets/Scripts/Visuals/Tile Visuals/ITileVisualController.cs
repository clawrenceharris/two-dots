using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileVisualController
{
    public void Init(Tile tile);
    public Tile Tile { get; }
    public TileVisuals Visuals { get; }
    public SpriteRenderer SpriteRenderer { get; }
    public IEnumerator BombHit();
    public IEnumerator Clear();
}
