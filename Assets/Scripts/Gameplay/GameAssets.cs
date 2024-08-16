using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [Header("Dots")]
    public NormalDot NormalDot;
   
    public SquareGem SquareGem;

    public RectangleGem RectangleGem;

    public NestingDot NestingDot;
    public Bomb Bomb;
    public ClockDot ClockDot;
    public AnchorDot AnchorDot;
    public BlankDot BlankDot;
    public BeetleDot BeetleDot;
    public MonsterDot MonsterDot;
    public LotusDot LotusDot;

    [Header("Tiles")]

    public Ice Ice;
    public EmptyTile EmptyTile;
    public Block Block;
    public OneSidedBlock OneSidedBlock;
    public Water Water;
    public Circuit Circuit;
    [Header("Lines")]
    public ConnectorLine Line;

    [Header("Sprites")]
    public Sprite[] Numbers;

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
            DotType.NestingDot => NestingDot,
            DotType.BeetleDot => BeetleDot,
            DotType.LotusDot => LotusDot,
            DotType.Gem => SquareGem,
            _ => null,
        };
    }

    public Tile FromTileType(TileType type)
    {
        return type switch
        {
            TileType.EmptyTile => EmptyTile,
            TileType.Block => Block,
            TileType.OneSidedBlock => OneSidedBlock,
            TileType.Ice => Ice,

            _ => null,
        };
    }
}
