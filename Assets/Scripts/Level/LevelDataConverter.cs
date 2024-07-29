using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LevelDataConverter : JsonConverter<LevelData>
{
    public override LevelData ReadJson(JsonReader reader, System.Type objectType, LevelData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Load JSON into a JObject to inspect its properties
        JObject jsonObject = JObject.Load(reader);
        // Deserialize common properties
        LevelData levelData = new()
        {
            levelNum = (int)jsonObject["levelNum"],
            width = (int)jsonObject["width"],
            height = (int)jsonObject["height"],
            moves = (int)jsonObject["moves"]
        };

        // Deserialize arrays
        JArray dotsToSpawnArray = (JArray)jsonObject["dotsToSpawn"];
        levelData.dotsToSpawn = DeserializeDotsArray(dotsToSpawnArray);

        JArray dotsOnBoardArray = (JArray)jsonObject["dotsOnBoard"];
        levelData.dotsOnBoard = DeserializeDotsArray(dotsOnBoardArray);

        
        JArray initialDotsToSpawn = (JArray)jsonObject["initialDotsToSpawn"];
        levelData.initialDotsToSpawn = DeserializeDotsArray(initialDotsToSpawn);

        
        JArray tilesOnBoardArray = (JArray)jsonObject["tilesOnBoard"];
        levelData.tilesOnBoard = DeserializeTilesArray(tilesOnBoardArray);

        return levelData;
    }
    private DotsGameObjectData[] DeserializeDotsArray(JArray array)

    {

        DotsGameObjectData[] deserializedArray = new DotsGameObjectData[array.Count];
        for (int i = 0; i < array.Count; i++)
        {
            JObject itemObject = (JObject)array[i];
            deserializedArray[i] = DotDataFactory.CreateDotData(itemObject);
        }

        return deserializedArray;
    }


    private DotsGameObjectData[] DeserializeTilesArray(JArray array)

    {

        DotsGameObjectData[] deserializedArray = new DotsGameObjectData[array.Count];
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