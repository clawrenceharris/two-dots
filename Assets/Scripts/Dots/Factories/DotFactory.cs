using UnityEngine;
using static Type;

public class DotFactory
{

    public static Dot CreateDot(DotToSpawnData dotData)
    {
        DotType dotType = JSONLevelLoader.FromJsonDotType(dotData.type);

        Dot dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));

        if (dot is IColorable colorDot)
        {
            ColorDotToSpawnData colorDotData = (ColorDotToSpawnData)dotData;

            colorDot.Color = JSONLevelLoader.FromJSONColor(colorDotData.color);
        }

        if (Type.HasNumber(dotType))
        {
            INumerable numberDot = (INumerable)dot;
            NumberDotToSpawnData numberDotData = (NumberDotToSpawnData)dotData;

            numberDot.InitialNumber = numberDotData.number;
        }



        return dot;
    }

    public static Dot CreateDot(DotData dotData)
    {
        DotType dotType = JSONLevelLoader.FromJsonDotType(dotData.type);
        Dot dot = Object.Instantiate(GameAssets.Instance.FromDotType(dotType));

        if (dotData is ColorDotData cDotData)
        {
            IColorable cDot = (IColorable)dot;
            cDot.Color = JSONLevelLoader.FromJSONColor(cDotData.color);
        }
        if (Type.HasNumber(dotType))
        {
            INumerable numberDot = (INumerable)dot;
            NumberDotData numberDotData = (NumberDotData)dotData;

            numberDot.InitialNumber = numberDotData.number;
        }



        return dot;
    }
}