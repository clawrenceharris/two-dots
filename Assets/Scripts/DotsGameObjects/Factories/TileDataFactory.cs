using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class TileDataFactory
{
    

    public static DotObject CreateTileData(JObject itemObject)
    {
        string type = (string)itemObject[LevelDataKeys.Type];
        JToken position = itemObject[LevelDataKeys.Position];
        JToken hitCount = itemObject[LevelDataKeys.HitCount];
        JToken isActive = itemObject[LevelDataKeys.Active];
        JToken direction = itemObject[LevelDataKeys.Direction];
        DotObject tileData = new(){
            hitCount = hitCount != null ? (int)hitCount : 0,
            type = type,
            col = position != null ? position.ToObject<int[]>()[0] : -1,
            row = position != null ? position.ToObject<int[]>()[1] : -1,
        };
        switch (type)
        {
            case LevelDataKeys.Types.OneSidedBlock:
                tileData.SetProperty(DotObject.Property.Directions, direction.ToObject<int[,]>());
                break;
            case LevelDataKeys.Types.Circuit:
                tileData.SetProperty(DotObject.Property.Active, (bool)isActive);
                break;
            
        };
        return tileData;
    }
}
