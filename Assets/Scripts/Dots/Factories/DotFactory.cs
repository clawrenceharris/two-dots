using UnityEngine;
using static Type;
using System;
using Object = UnityEngine.Object;

public class DotFactory
{
   

    public static Dot CreateDot(DotData data)
    {
        DotType dotType = JSONLevelLoader.FromJsonDotType(data.type);
        Dot dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));

        if (dot is IColorable colorableDot)
        {
            string color = data.GetProperty<string>("Color");
            colorableDot.Color = JSONLevelLoader.FromJSONColor(color);
        }

        if (dot is INumerable numberableDot)
        {
            int number = data.GetProperty<int>("Number");

            numberableDot.InitialNumber = number;
        }

        if (dot is IDirectional directionalDot)
        {
            int directionX = data.GetProperty<int>("DirectionX");
            int directionY = data.GetProperty<int>("DirectionY");

            directionalDot.DirectionX = directionX;
            directionalDot.DirectionY = directionY;

        }

        if(dot is IMulticolored multicolored)
        {
            string[] colors = data.GetProperty<string[]>("Colors");
            multicolored.Colors = new DotColor[colors.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                multicolored.Colors[i] = JSONLevelLoader.FromJSONColor(colors[i]);
            }
        }

        

        return dot;
    }

   
}