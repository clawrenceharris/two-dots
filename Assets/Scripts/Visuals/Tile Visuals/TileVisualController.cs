using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileVisualController : ITileVisualController
{
    public Tile Tile { get; protected set; }
    public TileVisuals Visuals { get; protected set; }

    public SpriteRenderer SpriteRenderer { get; protected set; }

    private Sprite sprite;


    public virtual void Init(Tile tile)
    {
        Tile = tile;
        Visuals = tile.GetComponent<TileVisuals>();
        SpriteRenderer = tile.GetComponent<SpriteRenderer>();
        sprite = SpriteRenderer.sprite;
        SetUp();

    }

    private void SetUp()
    {
        SpriteRenderer.color = ColorSchemeManager.FromTileType(Tile.TileType);
        Tile.gameObject.transform.localScale = Vector2.one * Board.offset;

    }


    public virtual IEnumerator BombHit()
    {
        SpriteRenderer.sprite =Visuals.bombHitSprite;
        yield return new WaitForSeconds(Visuals.clearTime);
        SpriteRenderer.sprite = sprite;
    }

    public IEnumerator Clear()
    {
        Tile.transform.DOScale(Vector2.zero, Visuals.clearTime);
        yield return null;
    }
}
