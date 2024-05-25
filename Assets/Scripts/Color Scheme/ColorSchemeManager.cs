using System;
using System.Net.NetworkInformation;
using UnityEngine;
using static Type;
using Color = UnityEngine.Color;

public class ColorSchemeManager
{
    private ColorScheme[] colorSchemes;
    public static ColorScheme CurrentColorScheme { get; private set; }

    public ColorSchemeManager(ColorScheme[] colorSchemes, int initialIndex)
    {
        this.colorSchemes = colorSchemes;
        SetColorScheme(initialIndex);
    }

    public void SetColorScheme(int index)
    {
        if (index >= 0 && index < colorSchemes.Length)
        {
            CurrentColorScheme = colorSchemes[index];
        }
        else
        {
            Debug.LogError("Invalid color scheme index.");
        }
    }


    public static Color FromDotColor(DotColor dotColor = DotColor.None)
    {
        return dotColor switch
        {
            DotColor.Red => CurrentColorScheme.red,
            DotColor.Yellow => CurrentColorScheme.yellow,
            DotColor.Green => CurrentColorScheme.green,
            DotColor.Blue => CurrentColorScheme.blue,
            DotColor.Purple => CurrentColorScheme.purple,


            _ => CurrentColorScheme.blank,
        };
    }

    public static Color FromTileType(TileType tileType)
    {
        Color bgColor = CurrentColorScheme.backgroundColor;
        return tileType switch
        {
            TileType.EmptyTile => new Color(bgColor.r, bgColor.g, bgColor.b, 0.5f),
            TileType.BlockTile => bgColor,
            TileType.OneSidedBlock => bgColor,


            _ => Color.white
        };
    }
}

