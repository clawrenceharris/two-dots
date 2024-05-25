using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class GameAssets : MonoBehaviour
{
    public Dot NormalDot;
    public Tile EmptyTile;
    public Tile BlockTile;
    public Tile OneSidedBlock;

    public Bomb Bomb;
    public ClockDot ClockDot;
    public Dot AnchorDot;
    public Dot BlankDot;
    public ConnectorLine Line;
    public static GameAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Dot FromDotType(DotType type)
    {
        return type switch
        {
            DotType.NormalDot => NormalDot,
            DotType.BlankDot => BlankDot,
            DotType.AnchorDot => AnchorDot,
            DotType.ClockDot => ClockDot,
            DotType.Bomb => Bomb,

            _ => null,
        };
    }

    public Tile FromTileType(TileType type)
    {
        return type switch
        {
            TileType.EmptyTile => EmptyTile,
            TileType.BlockTile => BlockTile,
            TileType.OneSidedBlock => OneSidedBlock,

            _ => null,
        };
    }
}
