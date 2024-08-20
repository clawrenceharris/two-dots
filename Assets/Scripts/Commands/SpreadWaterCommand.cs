using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
public class SpreadWaterCommand : Command
{
    public override CommandType CommandType => CommandType.SpreadWater;
    private readonly List<Dot> visitedDots = new();
    private readonly List<Water> visitedTiles = new();
    private readonly List<Water> originalWaterTiles = new();
     private readonly    List<Water> filledTiles = new();
    int ongoingCoroutines = 0;


    private void UpdateWaterTileSprites(Water waterTile, Board board){
        List<Water> neighbors = board.GetTileNeighbors<Water>(waterTile.Column, waterTile.Row, false);
        
        
        foreach(Water neighbor in neighbors){
            neighbor.GetVisualController<WaterVisualController>().UpdateHoleSprites(board);

        }
        waterTile.GetVisualController<WaterVisualController>().UpdateHoleSprites(board);


    }
    private IEnumerator DigWaterHoles(Water waterTile, Board board)
    {


        IEnumerable<Dot> neighbors = board.GetClearedDotNeighbors(waterTile.Column, waterTile.Row, false);
        List<Water> waterTiles = new();        
        foreach (Dot neighbor in neighbors)
        {
            if (visitedDots.Contains(neighbor) || board.GetTileAt(neighbor.Column, neighbor.Row) || board.GetDotAt(neighbor.Column, neighbor.Row))
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
            UpdateWaterTileSprites(newWaterTile, board);

        }

        yield return new WaitForSeconds(0.2f);

        foreach (Water w in waterTiles)
        {
            ongoingCoroutines++;

            CoroutineHandler.StartStaticCoroutine(DigWaterHoles(w, board));
        }

        ongoingCoroutines--;

    }
private List<Water> GetWaterPath(Water startingTile, Vector2Int direction, Board board, Action<Water> onValidNeighbor = null)
    {

        // Queue for breadth-first search traversal
        Queue<Water> queue = new();
        List<Water> path  = new();     
        queue.Enqueue(startingTile);
        path.Add(startingTile);
        // Perform breadth-first search
        while (queue.Count > 0)
        {
            Water currentTile = queue.Dequeue();
            Water neighbor = board.GetTileAt<Water>(currentTile.Column + direction.x, currentTile.Row + direction.y);
            

           

            if (neighbor == null || visitedTiles.Contains(neighbor))
            {
    
                break;
    
            }
            else{
                // Add the neighbor to the queue for further exploration
                queue.Enqueue(neighbor);
                path.Add(neighbor);
                visitedTiles.Add(neighbor);
                onValidNeighbor?.Invoke(neighbor);
            }
  
        }
        return path;
    }
    private IEnumerator FillWaterHoles(Water waterTile, Vector2Int direction, Board board)
    {
        List<Water> path;
        Vector2Int[] directions = {Vector2Int.right,Vector2Int.left, Vector2Int.up, Vector2Int.down,  };
       
            Water neighbor = board.GetTileAt<Water>(waterTile.Column + direction.x, waterTile.Row + direction.y);
            if (neighbor && !visitedTiles.Contains(neighbor))
            {
                
                path = GetWaterPath(waterTile, direction, board);
                for(int i = 0; i < path.Count -1; i++){
                    Water startTile = path[i];
                    Water endTile = path[i +1];
                    filledTiles.Add(startTile);
                    ongoingCoroutines++;

                    yield return CoroutineHandler.StartStaticCoroutine(startTile.GetVisualController<WaterVisualController>().FillWater(endTile, path.Count),()=>{

                        ongoingCoroutines--;
                    List<Water> neighbors = board.GetTileNeighbors<Water>(endTile.Column, endTile.Row);
                    if(neighbors.Any(neighbor => !visitedTiles.Contains(neighbor))){
                        foreach (Vector2Int direction in directions)
                        
                            CoroutineHandler.StartStaticCoroutine(FillWaterHoles(endTile, direction, board));
                    }
                    });
                    
                    if(i == path.Count - 2){
                        filledTiles.Add(endTile);

                    }
                   
                
                

                
                
            }
        }

        

    }

    

    public override IEnumerator Execute(Board board)
    {
        List<Water> waterTiles = board.FindTilesOfType<Water>();
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(SpreadWaterCommand));
        Vector2Int[] directions = {Vector2Int.right,Vector2Int.left, Vector2Int.up, Vector2Int.down,  };
         
        foreach(Water waterTile in waterTiles)
        {
            visitedTiles.Add(waterTile);
            ongoingCoroutines++;
            CoroutineHandler.StartStaticCoroutine(DigWaterHoles(waterTile, board));


        }
       
        yield return new WaitUntil(() => ongoingCoroutines == 0);
        ongoingCoroutines = 0;
        for(int col = 0; col < Board.Width; col++){

            for(int row = 0; row < Board.Height; row++){
                
                Water waterTile = waterTiles.FirstOrDefault((waterTile)=> waterTile.Column == col && waterTile.Row == row);
                if(waterTile){
                    foreach (Vector2Int direction in directions)

                        CoroutineHandler.StartStaticCoroutine(FillWaterHoles(waterTile,direction, board));     
                }
       
        

            }
           

           
        }
        // foreach(Water waterTile in waterTiles){
        //     foreach (Vector2Int direction in directions)
        //         CoroutineHandler.StartStaticCoroutine(FillWaterHoles(waterTile, direction, board));     
        // }
        yield return new WaitUntil(() => ongoingCoroutines == 0);
        foreach(Water visitedTile in visitedTiles){
            visitedTile.GetVisualController<WaterVisualController>().UpdateWaterSprites(board);
        }
        WaterVisualController.DestroyTempWaterSprites();        
        
        yield return base.Execute(board);
    }
}
