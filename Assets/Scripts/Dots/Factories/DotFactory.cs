using UnityEngine;
using static Type;
using System;
using Object = UnityEngine.Object;

public class DotFactory
{
   

    public static Dot CreateDot<T>(T dotData)
    {
        Dot dot = null;
        if (dotData is DotToSpawnData data) {
            DotType dotType = JSONLevelLoader.FromJsonDotType(data.type);
            dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));
        }

        if(dot == null)
        {
            throw new ArgumentException("The dot type to return could not be determined with the given data.");
        }

        if (dot is IColorable colorableDot)
        {
            IColorableData colorableData = (IColorableData)dotData;

            colorableDot.Color = JSONLevelLoader.FromJSONColor(colorableData.Color);
        }

        if (dot is INumerable numberableDot)
        {
            INumerableData numerableData = (INumerableData)dotData;

            numberableDot.InitialNumber = numerableData.Number;
        }

        if (dot is IDirectional directionalDot)
        {
            IDirectionalData directionalData = (IDirectionalData)dotData;

            directionalDot.DirectionX = directionalData.DirectionX;
            directionalDot.DirectionY = directionalData.DirectionY;

        }

        

        return dot;
    }

   
}