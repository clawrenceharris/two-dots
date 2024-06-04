using UnityEngine;
using static Type;

public class TileFactory
{

    public static Tile CreateTile(TileData tileData)
    {
        TileType tileType = JSONLevelLoader.FromJsonTileType(tileData.type);

        Tile tile = Object.Instantiate(GameAssets.Instance.FromTileType(tileType));

       
        if (tile is IDirectional directionalTile)
        {
            DirectionalTileData directionalTileData = (DirectionalTileData)tileData;

            directionalTile.DirectionX = directionalTileData.DirectionX;
            directionalTile.DirectionY = directionalTileData.DirectionY;

        }
        return tile;
    }

}