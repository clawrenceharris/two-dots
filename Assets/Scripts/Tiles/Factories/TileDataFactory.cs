using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static Type;

public class TileDataFactory
{
    

    public static TileData CreateTileData(JObject itemObject)
    {
        JToken type = itemObject["type"];
        JToken col = itemObject["col"];
        JToken row = itemObject["row"];
        JToken direction = itemObject["direction"];

        TileType tileType = JSONLevelLoader.FromJsonTileType((string)type);
        return tileType switch
        {
            TileType.OneSidedBlock => new DirectionalTileData()
            {
                type = (string)type,
                col = (int)col,
                row = (int)row,
                direction = direction.ToObject<Direction>()

            },
            _ => new TileData()
            {
                type = (string)type,
                col = (int)col,
                row = (int)row,
            },
        };
    }
}
