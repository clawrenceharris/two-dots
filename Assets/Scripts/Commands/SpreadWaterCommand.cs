using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Linq;
public class SpreadWaterCommand : Command
{
    public override CommandType CommandType => CommandType.SpreadWater;
    private readonly List<Dot> visitedDots = new();

    int ongoingCoroutines = 0;

    private IEnumerator DigWaterHoles(Water waterTile, Board board)
    {


        IEnumerable<Dot> neighbors = board.GetClearedDotNeighbors(waterTile.Column, waterTile.Row, false);
        List<Water> waterTiles = new();        
        foreach (Dot neighbor in neighbors)
        {
            if (visitedDots.Contains(neighbor) || board.GetTileAt(neighbor.Column, neighbor.Row))
            {
                continue;
            }
            DidExecute = true;

            visitedDots.Add(neighbor);
            DotsGameObjectData data = new(JSONLevelLoader.ToJsonTileType(TileType.Water))
            {
                col = neighbor.Column,
                row = neighbor.Row
            };
            Water newWaterTile = board.InitDotsGameObject<Water>(data);
            waterTiles.Add(newWaterTile);

        }
        yield return new WaitForSeconds(0.2f);

        foreach (Water w in waterTiles)
        {
            CoroutineHandler.StartStaticCoroutine(DigWaterHoles(w, board));
        }

        ongoingCoroutines--;

    }



    public override IEnumerator Execute(Board board)
    {
        List<Water> waterTiles = board.FindTilesOfType<Water>();
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(SpreadWaterCommand));

        foreach(Water waterTile in waterTiles)
        {

            ongoingCoroutines++;
            CoroutineHandler.StartStaticCoroutine(DigWaterHoles(waterTile, board));


        }
        yield return new WaitUntil(() => ongoingCoroutines == 0);
        yield return base.Execute(board);
    }
}
