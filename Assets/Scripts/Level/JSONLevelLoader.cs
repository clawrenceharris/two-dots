
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class LevelLoader
{
    public static LevelData Level;
    public static LevelData LoadLevelData(TextAsset textAsset){

        var settings = new JsonSerializerSettings
        {
            Converters = { new LevelDataConverter() }
        };
        Level = JsonConvert.DeserializeObject<LevelData>(textAsset.text, settings);
        return Level;
    }

    
    public static LevelData LoadLevelData(int levelNum)
    {
        string path = Application.dataPath + "/Json/World " + (Game.Instance.WorldIndex + 1) + "/level_" + levelNum + ".json";
        string json = File.ReadAllText(path);

        var settings = new JsonSerializerSettings
        {
            Converters = { new LevelDataConverter() }
        };
        LevelData level = JsonConvert.DeserializeObject<LevelData>(json, settings);
        return level;
    }
    public static DotsGameObject FromJsonType(string type)
    {
        //dots
        switch (type)
        {
            case LevelDataKeys.Types.Normal: return GameAssets.Instance.NormalDot;
            case LevelDataKeys.Types.Clock: return GameAssets.Instance.ClockDot;
            case LevelDataKeys.Types.Bomb: return GameAssets.Instance.Bomb;
            case LevelDataKeys.Types.Blank: return GameAssets.Instance.BlankDot;
            case LevelDataKeys.Types.Anchor: return GameAssets.Instance.AnchorDot;
            case LevelDataKeys.Types.Nesting: return GameAssets.Instance.NestingDot;
            case LevelDataKeys.Types.Beetle: return GameAssets.Instance.BeetleDot;
            case LevelDataKeys.Types.Monster: return GameAssets.Instance.MonsterDot;
            case LevelDataKeys.Types.Lotus: return GameAssets.Instance.LotusDot;
            case LevelDataKeys.Types.SquareGem: return GameAssets.Instance.SquareGem;
            case LevelDataKeys.Types.RectangleGem: return GameAssets.Instance.RectangleGem;
        };

        //tiles
        return type switch
        {
            LevelDataKeys.Types.OneSidedBlock => GameAssets.Instance.OneSidedBlock,
            LevelDataKeys.Types.EmptyTile => GameAssets.Instance.EmptyTile,
            LevelDataKeys.Types.Ice => GameAssets.Instance.Ice,
            LevelDataKeys.Types.Water => GameAssets.Instance.Water,
            LevelDataKeys.Types.Circuit => GameAssets.Instance.Circuit,
            _ => throw new ArgumentException( type +" is not a valid Json game object type"),
        };
    }




    public static TileType FromJsonTileType(string type)
    {
        return type switch
        {
            LevelDataKeys.Types.Ice => TileType.Ice,
            "slime" => TileType.Slime,
            LevelDataKeys.Types.Block => TileType.Block,
            LevelDataKeys.Types.OneSidedBlock => TileType.OneSidedBlock,
            LevelDataKeys.Types.EmptyTile => TileType.EmptyTile,
           LevelDataKeys.Types.Water => TileType.Water,
            LevelDataKeys.Types.Circuit => TileType.Circuit,
            _ => TileType.None,
        };
    }

   
    public static DotColor FromJsonColor(string color)
    {
        return color switch
        {
            LevelDataKeys.DotColors.Red => DotColor.Red,
            LevelDataKeys.DotColors.Blue => DotColor.Blue,
            LevelDataKeys.DotColors.Yellow => DotColor.Yellow,
            LevelDataKeys.DotColors.Purple => DotColor.Purple,
            LevelDataKeys.DotColors.Green => DotColor.Green,
            LevelDataKeys.DotColors.Blank => DotColor.Blank,
            _ => throw new ArgumentException(),
        };
    }

    public static string ToJsonDotType(DotType type)
    {
        return type switch
        {
            DotType.NormalDot => "normal",
            DotType.ClockDot => "clock",
            DotType.Bomb => LevelDataKeys.Types.Bomb,
            DotType.BlankDot => LevelDataKeys.Types.Blank,
            DotType.AnchorDot => LevelDataKeys.Types.Anchor,
            DotType.NestingDot => LevelDataKeys.Types.Nesting,
            DotType.BeetleDot => LevelDataKeys.Types.Beetle,
            DotType.SquareGem => LevelDataKeys.Types.SquareGem,
            DotType.RectangleGem => LevelDataKeys.Types.RectangleGem,
            _ => throw new ArgumentException(),
        };
    }
    public static string ToJsonTileType(TileType type)
    {
        return type switch
        {
            TileType.Slime => "slime",
            TileType.Ice => LevelDataKeys.Types.Ice,
            TileType.Block => LevelDataKeys.Types.Block,
            TileType.OneSidedBlock => LevelDataKeys.Types.OneSidedBlock,
            TileType.Water => LevelDataKeys.Types.Water,
            TileType.Circuit => LevelDataKeys.Types.Circuit,
            _ => throw new ArgumentException(),
        };
    }

    public static object ToJsonColor(DotColor color)
    {
        return color switch
        {
            DotColor.Red => LevelDataKeys.DotColors.Red,
            DotColor.Blue => LevelDataKeys.DotColors.Blue,
            DotColor.Yellow =>LevelDataKeys.DotColors.Yellow,
            DotColor.Purple => LevelDataKeys.DotColors.Purple,
            DotColor.Green => LevelDataKeys.DotColors.Green,
            DotColor.Blank => LevelDataKeys.DotColors.Blank,
            _ => DotColor.None,
        };
    }
}
