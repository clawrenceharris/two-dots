using UnityEngine;
using static Type;
using System;
using Object = UnityEngine.Object;

public class DotFactory
{
    public static T CreateDotsGameObject<T>(DotsGameObjectData data)
        where T : DotsGameObject
    {
        
        DotsGameObject dotsObject = Object.Instantiate(JSONLevelLoader.FromJsonType(data.type));


        if(dotsObject is IHittable hittable)
        {
            hittable.HitCount = data.hitCount;

        }

        if (dotsObject is IColorable colorable)
        {
            string color = data.GetProperty<string>("Color");
            colorable.Color = JSONLevelLoader.FromJSONColor(color);
        }

        if (dotsObject is INumerable numberable)
        {
            int number = data.GetProperty<int>("Number");

            numberable.InitialNumber = number;
        }

        if (dotsObject is IDirectional directional)
        {
            int directionX = data.GetProperty<int>("DirectionX");
            int directionY = data.GetProperty<int>("DirectionY");

            directional.DirectionX = directionX;
            directional.DirectionY = directionY;

        }

        if(dotsObject is IMulticolored multicolored)
        {
            string[] colors = data.GetProperty<string[]>("Colors");
            multicolored.Colors = new DotColor[colors.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                multicolored.Colors[i] = JSONLevelLoader.FromJSONColor(colors[i]);
            }
        }

        

        return (T)dotsObject;
    }

}