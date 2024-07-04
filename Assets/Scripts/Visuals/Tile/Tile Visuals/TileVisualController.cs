using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static Type;

public abstract class TileVisualController : VisualController, ITileVisualController
{
    protected override void SetUp()
    {
        GetGameObject<Tile>().transform.localScale = Vector2.one * Board.offset;
        base.SetUp();
    }
}