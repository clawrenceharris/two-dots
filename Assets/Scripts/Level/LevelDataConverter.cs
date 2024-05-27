using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Type;

public class LevelDataConverter : JsonConverter<LevelData>
{
    public override LevelData ReadJson(JsonReader reader, System.Type objectType, LevelData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Load JSON into a JObject to inspect its properties
        JObject jsonObject = JObject.Load(reader);
        // Deserialize common properties
        LevelData levelData = new LevelData();
        levelData.levelNum = (int)jsonObject["levelNum"];
        levelData.width = (int)jsonObject["width"];
        levelData.height = (int)jsonObject["height"];
        levelData.moves = (int)jsonObject["moves"];

        // Deserialize dotsToSpawn array
        JArray dotsToSpawnArray = (JArray)jsonObject["dotsToSpawn"];
        levelData.dotsToSpawn = DeserializeDotsToSpawn(dotsToSpawnArray);
        // Deserialize dotsOnBoard array
        JArray dotsOnBoardArray = (JArray)jsonObject["dotsOnBoard"];
        levelData.dotsOnBoard = DeserializeDotsOnBoardData(dotsOnBoardArray);

        

        

        // Deserialize tilesOnBoard array
        JArray tilesOnBoardArray = (JArray)jsonObject["tilesOnBoard"];
        levelData.tilesOnBoard = DeserializeTilesArray(tilesOnBoardArray);

        return levelData;
    }
    private DotData[] DeserializeDotsOnBoardData(JArray array)

    {

        DotData[] deserializedArray = new DotData[array.Count];
        for (int i = 0; i < array.Count; i++)
        {
            JObject itemObject = (JObject)array[i];
            deserializedArray[i] = DotDataFactory.CreateDotData(itemObject);
        }

        return deserializedArray;
    }


    private DotToSpawnData[] DeserializeDotsToSpawn(JArray array)

    {

        DotToSpawnData[] deserializedArray = new DotToSpawnData[array.Count];
        for (int i = 0; i < array.Count; i++)
        {
            JObject itemObject = (JObject)array[i];
            deserializedArray[i] = DotDataFactory.CreateDotToSpawnData(itemObject);
        }

        return deserializedArray;
    }

    private TileData[] DeserializeTilesArray(JArray array)

    {

        TileData[] deserializedArray = new TileData[array.Count];
        for (int i = 0; i < array.Count; i++)
        {
            JObject itemObject = (JObject)array[i];
            deserializedArray[i] = TileDataFactory.CreateTileData(itemObject);
        }

        return deserializedArray;
    }

    public override void WriteJson(JsonWriter writer, LevelData value, JsonSerializer serializer)
    {

        throw new NotImplementedException();

    }
}