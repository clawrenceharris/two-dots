using UnityEngine;
using static Type;
using System;
using Object = UnityEngine.Object;

public class DotFactory
{
    public static T CreateDotsGameObject<T>(DotsGameObjectData data)
        where T : DotsGameObject
    {
        
        DotsGameObject dotsGameObject = Object.Instantiate(JSONLevelLoader.FromJsonType(data.type));

        if (dotsGameObject is IHittable hittable)
        {
            hittable.HitCount = data.hitCount;

        }

        if (dotsGameObject is IColorable colorable)
        {
            string color = data.GetProperty<string>("Color");
            colorable.Color = JSONLevelLoader.FromJSONColor(color);
            Debug.Log("HIIIII");
        }

        if (dotsGameObject is INumerable numberable)
        {
            int number = data.GetProperty<int>("Number");

            numberable.InitialNumber = number;
        }

        if (dotsGameObject is IDirectional directional)
        {
            int directionX = data.GetProperty<int>("DirectionX");
            int directionY = data.GetProperty<int>("DirectionY");

            directional.DirectionX = directionX;
            directional.DirectionY = directionY;

        }

        if(dotsGameObject is IMulticolorable multicolored)
        {
            string[] colors = data.GetProperty<string[]>("Colors");
            multicolored.Colors = new DotColor[colors.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                multicolored.Colors[i] = JSONLevelLoader.FromJSONColor(colors[i]);
            }
        }

        

        return (T)dotsGameObject;
    }

}