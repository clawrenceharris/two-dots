using UnityEngine;
using static Type;

public class TileFactory
{

    public static Tile CreateTile(DotsObjectData data)
    {
        TileType tileType = JSONLevelLoader.FromJsonTileType(data.type);

        Tile tile = Object.Instantiate(GameAssets.Instance.FromTileType(tileType));

       
        if (tile is IDirectional directionalTile)
        {

            directionalTile.DirectionX = data.GetProperty<int>("DirectionX");
            directionalTile.DirectionY = data.GetProperty<int>("DirectionY");

        }
        return tile;
    }

}