using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Debug = UnityEngine.Debug;

public class DotDataFactory
{

    public static DotObject CreateDotData(JObject itemObject)
    {
        string type = (string)itemObject[LevelDataKeys.Type];
        JToken position = itemObject[LevelDataKeys.Position];
        JToken color = itemObject[LevelDataKeys.Color];
        JToken hitCount = itemObject[LevelDataKeys.HitCount];
        JToken direction = itemObject[LevelDataKeys.Direction];
        JToken number = itemObject[LevelDataKeys.Number];
        DotObject dotData = new()
        {
            type = type,
            hitCount = hitCount != null ? (int)hitCount : 0,
            col = position != null ? position.ToObject<int[]>()[0]  : -1,
            row = position != null ? position.ToObject<int[]>()[1]  : -1,

        };
            Debug.Log(color);
        switch (type)
        {
            case LevelDataKeys.Types.Lotus:
            case LevelDataKeys.Types.Normal:
                dotData.SetProperty(DotObject.Property.Color, (string)color);
                dotData.SetProperty(DotObject.Property.Color, (string)color);
                dotData.SetProperty(DotObject.Property.Color, (string)color);
                break;
            case LevelDataKeys.Types.Beetle:
                dotData.SetProperty(DotObject.Property.Color, (string)color);
                dotData.SetProperty(DotObject.Property.Directions, direction.ToObject<int[,]>());
                break;
            case LevelDataKeys.Types.Clock:
                dotData.SetProperty(DotObject.Property.Number, (int)number);
                dotData.SetProperty(DotObject.Property.Color, LevelDataKeys.DotColors.Blank);

                break;
            case LevelDataKeys.Types.Monster:
                dotData.SetProperty(DotObject.Property.Color, (string)color);
                dotData.SetProperty(DotObject.Property.Number, (int)number);

                break;
            case LevelDataKeys.Types.Blank:
                dotData.SetProperty(DotObject.Property.Color, LevelDataKeys.DotColors.Blank);
                break;
            case LevelDataKeys.Types.SquareGem:
                

                dotData.SetProperty(DotObject.Property.Directions, direction.ToObject<int[,]>());
                break;
        };
        return dotData;
    }
    
    
}
