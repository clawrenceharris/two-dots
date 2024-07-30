using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class TileDataFactory
{
    

    public static DotsGameObjectData CreateTileData(JObject itemObject)
    {
        string type = (string)itemObject["type"];
        JToken col = itemObject["col"];
        JToken row = itemObject["row"];
        JToken hitCount = itemObject["hitCount"];

        JToken directionX = itemObject["directionX"];
        JToken directionY = itemObject["directionY"];
        JToken isActive = itemObject["isActive"];
        DotsGameObjectData dotData = new(type)
        {
            hitCount = hitCount != null ? (int)hitCount : 0,
            col = col != null ? (int)col : -1,
            row = row != null ? (int)row : -1,

        };

        switch (type)
        {
            case "one sided block":
                dotData.SetProperty("DirectionX", (int)directionX);
                dotData.SetProperty("DirectionY", (int)directionY);
                break;
            case "circuit":
                dotData.SetProperty("IsActive", (bool)isActive);
                break;
        };
        return dotData;
    }
}
