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
        JToken type = itemObject["type"];
        JToken color = itemObject["color"];
        JToken col = itemObject["col"];
        JToken row = itemObject["row"];
        JToken directionX = itemObject["directionX"];
        JToken directionY = itemObject["directionY"];
        JToken number = itemObject["number"];


        DotType dotType = JSONLevelLoader.FromJsonDotType((string)type);
        return dotType switch
        {
            DotType.NormalDot => new ColorableDotData()
            {
                Color = (string)color,
                type = (string)type,
                col = (int)col,
                row = (int)row
            },
            DotType.ClockDot => new NumerableDotData()
            {
                Number = (int)number,
                type = (string)type,
                col = (int)col,
                row = (int)row

            },
            DotType.BeetleDot => new DirectionalColorDotData()
            {
                type = (string)type,
                col = (int)col,
                row = (int)row,
                Color = (string)color,
                DirectionX = (int)directionX,
                DirectionY = (int)directionY

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
        JToken directionX = itemObject["directionX"];
        JToken directionY = itemObject["directionY"];

        DotType dotType = JSONLevelLoader.FromJsonDotType((string)type);
        return dotType switch
        {
            DotType.NormalDot => new ColorableDotToSpawnData()
            {
                type = (string)type,
                Color = (string)color
            },
            DotType.ClockDot => new NumberDotToSpawnData()
            {
                type = (string)type,
                Number = (int)number

            },
            DotType.BeetleDot => new DirectionalColorDotToSpawnData()
            {
                type = (string)type,
                Color = (string)color,
                DirectionX = (int)directionX,
                DirectionY = (int)directionY

            },
            _ => new DotToSpawnData()
            {
                type = (string)type,
            },
           
        };
    }


    
}
