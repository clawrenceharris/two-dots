using UnityEngine;
using static Type;
using System;
using Object = UnityEngine.Object;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DotFactory
{
   

    public static Dot CreateDot(DotsObjectData data)
    {
        
<<<<<<< Updated upstream
        DotType dotType = JSONLevelLoader.FromJsonDotType(data.type);
        Dot dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));

        dot.HitCount = data.hitCount;

        if (dot is IColorable colorableDot)
=======
        DotsGameObject dotsGameObject = Object.Instantiate(JSONLevelLoader.FromJsonType(data.type));

        if (dotsGameObject is IHittable hittable)
        {
            hittable.HitCount = data.hitCount;

        }

        if (dotsGameObject is IColorable colorable)
>>>>>>> Stashed changes
        {
            string color = data.GetProperty<string>("Color");
            colorableDot.Color = JSONLevelLoader.FromJSONColor(color);
        }

<<<<<<< Updated upstream
        if (dot is INumerable numberableDot)
=======
        if (dotsGameObject is INumerable numberable)
>>>>>>> Stashed changes
        {
            int number = data.GetProperty<int>("Number");

            numberableDot.InitialNumber = number;
        }

<<<<<<< Updated upstream
        if (dot is IDirectional directionalDot)
=======
        if (dotsGameObject is IDirectional directional)
>>>>>>> Stashed changes
        {
            int directionX = data.GetProperty<int>("DirectionX");
            int directionY = data.GetProperty<int>("DirectionY");

            directionalDot.DirectionX = directionX;
            directionalDot.DirectionY = directionY;

        }

<<<<<<< Updated upstream
        if(dot is IMulticolored multicolored)
=======
        if(dotsGameObject is IMulticolorable multicolored)
>>>>>>> Stashed changes
        {
            string[] colors = data.GetProperty<string[]>("Colors");
            multicolored.Colors = new DotColor[colors.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                multicolored.Colors[i] = JSONLevelLoader.FromJSONColor(colors[i]);
            }
        }

        

<<<<<<< Updated upstream
        return dot;
=======
        return (T)dotsGameObject;
>>>>>>> Stashed changes
    }

    public static Dot CreateBomb(int col, int row)
    {
        Bomb bomb = Object.Instantiate(GameAssets.Instance.Bomb);
        bomb.Init(col, row);
        return bomb;

    }
}