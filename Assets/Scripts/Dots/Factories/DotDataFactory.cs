using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static Type;

public class DotDataFactory
{   
    public static DotData CreateDotData(JObject itemObject)
    {
        JToken type = itemObject["type"];// Assuming "color" is the key for color property in the JObject
        JToken number = itemObject["number"];// Assuming "color" is the key for color property in the JObject
        JToken color = itemObject["color"];// Assuming "color" is the key for color property in the JObject
        JToken col = itemObject["col"];// Assuming "color" is the key for color property in the JObject
        JToken row = itemObject["row"];// Assuming "color" is the key for color property in the JObject

        DotType dotType = JSONLevelLoader.FromJsonDotType((string)type);
        return dotType switch
        {
            DotType.NormalDot => new ColorDotData()
            {
                color = (string)color,
                type = (string)type,
                col = (int)col,
                row = (int)row
            },
            DotType.ClockDot => new NumberDotData()
            {
                number = (int)number,
                type = (string)type,
                col = (int)col,
                row = (int)row

            },
            _ => new DotData()
            {
                type = (string)type,
                col = (int)col,
                row = (int)row
            },
        };
    }
    public static DotToSpawnData CreateDotToSpawnData(JObject itemObject)
    {
        JToken type = itemObject["type"];// Assuming "color" is the key for color property in the JObject
        JToken number = itemObject["number"];// Assuming "color" is the key for color property in the JObject
        JToken color = itemObject["color"];// Assuming "color" is the key for color property in the JObject

        DotType dotType = JSONLevelLoader.FromJsonDotType((string)type);
        switch (dotType)
        {
            case DotType.NormalDot:
                return new ColorDotToSpawnData() {
                    type = (string)type,
                    color = (string)color
                };
            case DotType.ClockDot:
                return new NumberDotToSpawnData() {
                    type = (string)type,
                    number = (int)number
                    
                };
            
            default:
                return new DotToSpawnData()
                {
                    type = (string)type,
                };
        }
    }


    
}
