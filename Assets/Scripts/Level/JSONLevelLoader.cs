
using UnityEngine;
using System.IO;
using static Type;
using Newtonsoft.Json;
using System;

public class JSONLevelLoader
{
    

    public static LevelData ReadJsonFile(int levelNum)
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

    public static DotsGameObject FromJsonType(string dotType)
    {
        return dotType switch
        {
            "normal" => GameAssets.Instance.NormalDot,
            "clock" => GameAssets.Instance.ClockDot,
            "bomb" => GameAssets.Instance.Bomb,
            "blank" => GameAssets.Instance.BlankDot,
            "anchor" => GameAssets.Instance.AnchorDot,
            "nesting" => GameAssets.Instance.NestingDot,
            "beetle" => GameAssets.Instance.BeetleDot,
            "monster" => GameAssets.Instance.MonsterDot,
            "one sided block" => GameAssets.Instance.OneSidedBlock,
            "empty" => GameAssets.Instance.EmptyTile,


            _ => throw new ArgumentException("'" + dotType +  "' is not a valid game object type"),
        };
    }



   
    

    public static TileType FromJsonTileType(string type)
    {
        return type switch
        {
            "empty" => TileType.EmptyTile,
            "block" => TileType.BlockTile,
            "one sided block" => TileType.OneSidedBlock,

            _ => TileType.None,
        };
    }

    public static BoardMechanicType FromJsonBoardMechanicType(string type)
    {
        return type switch
        {
            "block" => BoardMechanicType.Ice,
            "slime" => BoardMechanicType.Slime,
            _ => BoardMechanicType.None,
        };
    }

    public static DotColor FromJSONColor(string color)
    {
        switch (color)
        {
            case "red": return DotColor.Red;
            case "blue": return DotColor.Blue;
            case "yellow": return DotColor.Yellow;
            case "purple": return DotColor.Purple;
            case "green": return DotColor.Green;
            default: return DotColor.Blank;
        }
    }

    public static string ToJsonDotType(DotType dotType)
    {
        return dotType switch
        {
            DotType.NormalDot => "normal",
            DotType.ClockDot => "clock",
            DotType.Bomb => "bomb",
            DotType.BlankDot => "blank",
            DotType.AnchorDot => "anchor",
            DotType.NestingDot => "nesting",
            DotType.BeetleDot => "beetle",
            _ => "bombb",
        };
    }
}
