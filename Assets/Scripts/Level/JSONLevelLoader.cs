
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEditor;

public class JSONLevelLoader
{
    public static LevelData LoadLevelData(TextAsset textAsset){

        var settings = new JsonSerializerSettings
        {
            Converters = { new LevelDataConverter() }
        };
        LevelData level = JsonConvert.DeserializeObject<LevelData>(textAsset.text, settings);
        return level;
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
            case "normal": return GameAssets.Instance.NormalDot;
            case "clock": return GameAssets.Instance.ClockDot;
            case "bomb": return GameAssets.Instance.Bomb;
            case "blank": return GameAssets.Instance.BlankDot;
            case "anchor": return GameAssets.Instance.AnchorDot;
            case "nesting": return GameAssets.Instance.NestingDot;
            case "beetle": return GameAssets.Instance.BeetleDot;
            case "monster": return GameAssets.Instance.MonsterDot;
            case "lotus": return GameAssets.Instance.LotusDot;
            case "gem": return GameAssets.Instance.SquareGem;
            case "r-gem": return GameAssets.Instance.RectangleGem;
        };

        //tiles
        return type switch
        {
            "one sided block" => GameAssets.Instance.OneSidedBlock,
            "empty" => GameAssets.Instance.EmptyTile,
            "ice" => GameAssets.Instance.Ice,
            "water" => GameAssets.Instance.Water,
            "circuit" => GameAssets.Instance.Circuit,
            _ => throw new ArgumentException("'" + type + "' is not a valid Json game object type"),
        };
    }




    public static TileType FromJsonTileType(string type)
    {
        return type switch
        {
            "ice" => TileType.Ice,
            "slime" => TileType.Slime,
            "block" => TileType.Block,
            "one sided block" => TileType.OneSidedBlock,
            "empty" => TileType.EmptyTile,
            "water" => TileType.Water,
            "circuit" => TileType.Circuit,
            _ => TileType.None,
        };
    }

   
    public static DotColor FromJsonColor(string color)
    {
        return color switch
        {
            "red" => DotColor.Red,
            "blue" => DotColor.Blue,
            "yellow" => DotColor.Yellow,
            "purple" => DotColor.Purple,
            "green" => DotColor.Green,
            "blank" => DotColor.Blank,
            _ => throw new ArgumentException(),
        };
    }

    public static string ToJsonDotType(DotType type)
    {
        return type switch
        {
            DotType.NormalDot => "normal",
            DotType.ClockDot => "clock",
            DotType.Bomb => "bomb",
            DotType.BlankDot => "blank",
            DotType.AnchorDot => "anchor",
            DotType.NestingDot => "nesting",
            DotType.BeetleDot => "beetle",
            DotType.SquareGem => "gem",
            DotType.RectangleGem => "r-gem",
            _ => throw new ArgumentException(),
        };
    }
    public static string ToJsonTileType(TileType type)
    {
        return type switch
        {
            TileType.Slime => "slime",
            TileType.Ice => "ice",
            TileType.Block => "block",
            TileType.OneSidedBlock => "one sided block",
            TileType.Water => "water",
            TileType.Circuit => "circuit",
            _ => throw new ArgumentException(),
        };
    }

    public static object ToJsonColor(DotColor color)
    {
        return color switch
        {
            DotColor.Red => "red",
            DotColor.Blue => "blue",
            DotColor.Yellow => "yellow",
            DotColor.Purple => "purple",
            DotColor.Green => "green",
            DotColor.Blank => "blank",
            _ => DotColor.None,
        };
    }
}
