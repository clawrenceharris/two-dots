
using UnityEngine;
using System.IO;
using static Type;
using Newtonsoft.Json;
using UnityEngine.UI;

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

    public static DotType FromJsonDotType(string dotType)
    {
        return dotType switch
        {
            "normal" => DotType.NormalDot,
            "clock" => DotType.ClockDot,
            "bomb" => DotType.Bomb,
            "blank" => DotType.BlankDot,
            "anchor" => DotType.AnchorDot,


            _ => DotType.None,
        };
    }
    public static Dot FromJsonDot(DotData dotData)
    {
        DotType dotType = FromJsonDotType(dotData.type);
        Dot dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));
        return dot;
    }

    public static Dot FromJsonDot(DotToSpawnData dotData)
    {
        DotType dotType = FromJsonDotType(dotData.type);
        Dot dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));
        return dot;
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

    
}
