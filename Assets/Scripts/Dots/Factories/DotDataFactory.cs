using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static Type;
public class DotDataFactory
{
    public static DotsObjectData CreateDotData(JObject itemObject)
    {
        string type = (string)itemObject["type"];
        JToken col = itemObject["col"];
        JToken row = itemObject["row"];
        JToken color = itemObject["color"];
        JToken colors = itemObject["colors"];
        JToken hitCount = itemObject["hitCount"];
        
        JToken directionX = itemObject["directionX"];
        JToken directionY = itemObject["directionY"];
        JToken number = itemObject["number"];
        DotsObjectData dotData = new(type)
        {
            hitCount = hitCount != null ? (int)hitCount : 0,
            col = col != null ? (int)col : -1,
            row = row != null ? (int)row : -1,

        };
       
        switch (type)
        {
            case "normal":
                dotData.SetProperty("Color", (string)color);
                break;
            case "beetle":
                dotData.SetProperty("Colors", colors.ToObject<string[]>());
                dotData.SetProperty("Color", (string)color);

                dotData.SetProperty("DirectionX", (int)directionX);
                dotData.SetProperty("DirectionY", (int)directionY);
                break;
            case "clock":
                dotData.SetProperty("Number", (int)number);
                break;
            case "monster":
                dotData.SetProperty("Color", (string)color);
                dotData.SetProperty("Number", (int)number);

                break;
            case "blank":
                dotData.SetProperty("Color", JSONLevelLoader.ToJsonDotType(DotType.BlankDot));

                break;

        };
        return dotData;
    }
    
    
}
