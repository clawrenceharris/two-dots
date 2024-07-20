using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class TileVisualController : VisualController, ITileVisualController
{
    public override void Init(DotsGameObject dotsGameObject)
    {
        SetUp();
    }

    protected override void SetUp()
    {
        GetGameObject<Tile>().transform.localScale = Vector2.one * Board.offset;
        base.SetUp();
    }
}