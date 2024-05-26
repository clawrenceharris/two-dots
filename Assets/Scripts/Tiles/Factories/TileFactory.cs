using UnityEngine;
using static Type;

public class TileFactory
{

    public static Tile CreateTile(TileData tileData)
    {
        TileType tileType = JSONLevelLoader.FromJsonTileType(tileData.type);

        Tile tile = Object.Instantiate(GameAssets.Instance.FromTileType(tileType));

       
        if (Type.HasDirection(tileType))
        {
            IDirectional directionalTile = (IDirectional)tile;
            DirectionalTileData directionalTileData = (DirectionalTileData)tileData;

            directionalTile.DirectionX = directionalTileData.direction.x;
            directionalTile.DirectionY = directionalTileData.direction.y;

        }
        return tile;
    }

}