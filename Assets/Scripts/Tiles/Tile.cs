using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static Type;
public abstract class Tile : MonoBehaviour, IBoardElement
{
    private int row;
    private int column;

    public static event Action<Tile> onTileCleared;
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }
    public abstract TileType TileType { get;}
    public ITileVisualController visualController;

    public virtual void Init(int column, int row)
    {
        this.column = column;
        this.row = row;
        InitDisplayController();
    }

    public virtual void InitDisplayController()
    {
        visualController = new TileVisualController();
        visualController.Init(this);
    }


    public virtual void Debug()
    {
        Debug(Color.black);
    }

    public virtual void Debug(Color color)
    {
        visualController.SpriteRenderer.color = color;
    }


    public virtual IEnumerator Clear()
    {
        yield return visualController.Clear();
        NotifyTileCleared();

    }
    protected void NotifyTileCleared()
    {
        onTileCleared?.Invoke(this);
    } 



}
