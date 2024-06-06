using UnityEngine;
using static Type;
using System;
using Object = UnityEngine.Object;

public class DotFactory
{
   

    public static Dot CreateDot<T>(T data)
    {
        DotToSpawnData dotToSpawnData = data as DotToSpawnData;
        DotType dotType = JSONLevelLoader.FromJsonDotType(dotToSpawnData.type);
        Dot dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));

        if (dot is IColorable colorableDot)
        {
            IColorableData colorableData = (IColorableData)dotToSpawnData;

            colorableDot.Color = JSONLevelLoader.FromJSONColor(colorableData.Color);
        }

        if (dot is INumerable numberableDot)
        {
            INumerableData numerableData = (INumerableData)dotToSpawnData;

            numberableDot.InitialNumber = numerableData.Number;
        }

        if (dot is IDirectional directionalDot)
        {
            IDirectionalData directionalData = (IDirectionalData)dotToSpawnData;

            directionalDot.DirectionX = directionalData.DirectionX;
            directionalDot.DirectionY = directionalData.DirectionY;

        }

        

        return dot;
    }

   
}