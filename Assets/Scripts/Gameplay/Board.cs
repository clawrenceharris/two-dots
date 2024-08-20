using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public class Board : MonoBehaviour
{
    public static int Width { get; private set; }
    public static int Height { get; private set; }
    private Dot[,] Dots;

    private Tile[,] Tiles;

    private DotsGameObjectData[] tilesOnBoard;
    private DotsGameObjectData[] initialDotsToSpawn;
    private DotsGameObjectData[] dotsToSpawn;
    private DotsGameObjectData[] dotsOnBoard;

    public static float offset = 2.5f;
    public static event Action<DotsGameObject> onObjectSpawned;
    public static float DotDropSpeed { get; private set; } = 0.45f;
    public List<Dot> ClearedDots { get; private set; } = new();

    public static event Action<Board> onBoardCreated;


    public void Init(LevelData level)
    {

        Width = level.width;
        Height = level.height;
        dotsOnBoard = level.dotsOnBoard;
        dotsToSpawn = level.dotsToSpawn;
        initialDotsToSpawn = level.initialDotsToSpawn;
        Dots = new Dot[level.width, level.height];
        Tiles = new Tile[level.width, level.height];
        tilesOnBoard = level.tilesOnBoard;
        SetUpBoard();


    }
    private void SetUpBoard()
    {
        InitTiles();
        InitDots();
        FillBoard(initialDotsToSpawn);
        onBoardCreated?.Invoke(this);

    }

    public bool HasAny<T>()
    {
        if(typeof(T) == typeof(Dot) || typeof(T).IsSubclassOf(typeof(Dot))){
            foreach (Dot dot in Dots)
            {
                if (dot is T)
                {
                    return true;
                }
            }
        }
           
        else if(typeof(T) == typeof(Tile) || typeof(T).IsSubclassOf(typeof(Tile))){
            foreach (Tile tile in Tiles)
            {
                if (tile is T)
                {
                    return true;
                }
            }
        }
        return false;
    }


    private void Start()
    {
        DotsGameObjectEvents.onCleared += OnCleared;
        CommandInvoker.onCommandsEnded += OnCommandsEnded;
    }

   
    private void OnCommandsEnded()
    {
        foreach(IHittable hittable in ClearedDots)
            DestroyDotsGameObject((DotsGameObject)hittable);
        ClearedDots.Clear();
    }

    private void OnCleared(DotsGameObject dotsGameObject)
    {
        DotsGameObject replacement = null;
        if (dotsGameObject is Dot dot)
        {
            /// replace the dot that is being cleared with its replacement dot
            replacement = InitDotsGameObject<Dot>(dotsGameObject.Replacement);
            if(replacement == null){
                Dots[dot.Column, dot.Row] = null;
            }
            ClearedDots.Add(dot);

        }
        if (dotsGameObject is Tile tile)
        {
            replacement = InitDotsGameObject<Tile>(dotsGameObject.Replacement);
            if(replacement == null){
                Tiles[tile.Column, tile.Row] = null;
            }

        }
        
        
    }

   

    public void DestroyDotsGameObject(DotsGameObject dotsGameObject)
    {
        Destroy(dotsGameObject.gameObject);
    }

    private void DropDot(Dot dot, int row){
        dot.Row = row;
        dot.transform.DOMoveY(row * offset, DotDropSpeed).SetEase(Ease.OutBounce);
                
    }
    
    private void InitDots()
    {
        for (int i = 0; i < dotsOnBoard.Length; i++)
        {
            Dot dot = InitDotsGameObject<Dot>(dotsOnBoard[i]);
            DropDot(dot, dot.Row);


        }
    }


    private void InitTiles()
    {
        for (int i = 0; i < tilesOnBoard.Length; i++)
        {
            InitDotsGameObject<Tile>(tilesOnBoard[i]);

        }
    }


    
    public void SpawnBomb(int col, int row)
    {
        if(Dots[col, row] is Bomb){
            return;
        }
        Bomb bomb = Instantiate(GameAssets.Instance.Bomb);
        bomb.transform.position = new Vector2(col, row) * offset;
        bomb.transform.parent = transform;
        Dots[col, row] = bomb;
        bomb.Init(col, row);

    }


    public IExplodable GetExplodableAt(int col, int row)
    {

        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            if (Dots[col, row] is IExplodable dot)
            {
                return dot;
            }
            else if (Tiles[col, row] is IExplodable tile)
            {
                return tile;
            }
        }
        return null;
    }


    public T InitDotsGameObject<T>(DotsGameObjectData data)
        where T : DotsGameObject
    {

        T dotsGameObject = default;

        if (data != null)
        {
            dotsGameObject = DotFactory.CreateDotsGameObject<T>(data);
            dotsGameObject.transform.position = new Vector2(data.col, data.row) * offset;
            dotsGameObject.transform.parent = transform;
            dotsGameObject.name = data.type + " (" + data.col + ", " + data.col + ")";
            dotsGameObject.Init(data.col, data.row);

            
            Put(dotsGameObject, data.col, data.row);
            onObjectSpawned?.Invoke(dotsGameObject);
        }

        return dotsGameObject;

    }


    private Dot InitRandomDot(int col, int row, DotsGameObjectData[] dotsToSpawn)
    {

        int randDot = UnityEngine.Random.Range(0, dotsToSpawn.Length);

        DotsGameObjectData dotData = dotsToSpawn[randDot];

        Dot dot = DotFactory.CreateDotsGameObject<Dot>(dotData);

        dot.transform.parent = transform;
        dot.name = dotData.type + " (" + col + ", " + row + ")";
        dot.transform.position = new Vector2(col, row + 100) * offset;
        dot.Init(col, row);
        Dots[col, row] = dot;

        return dot;
    }


    

    public void Remove<T>(T dotsGameObject)
        where T : DotsGameObject
    {

        if (dotsGameObject is Dot dot)
            Dots[dot.Column, dot.Row] = null;

        if (dotsGameObject is Tile tile)
        {
            Tiles[tile.Column, tile.Row] = null;
        }

    }

    public void Remove<T>(T dotsGameObject, int col, int row)
        where T : DotsGameObject
    {

        if (dotsGameObject is Dot)
            Dots[col, row] = null;

        if (dotsGameObject is Tile)
        {
            Tiles[col, row] = null;
        }

    }
   
    /// <summary>
    /// Returns a list of neighboring dots that surrounds
    /// an element at the given column and row
    /// </summary>
    /// <typeparam name="T">A reference</typeparam>
    /// <param name="col">The column of the board element whose neighbors are being found</param>
    /// <param name="row">The row of the board element whose neighbors are being found </param>
    /// <param name="includesDiagonals">Whether or not the method should return diagonal neighbors as well
    /// <returns>A list of the neighboring board elements </returns>
    public List<T> GetDotNeighbors<T>(int col, int row, bool includesDiagonals = true)
        where T : class
    {

        List<T> neighbors = new()
        {
            GetDotAt<T>(col, row + 1),
            GetDotAt<T>(col, row - 1),
            GetDotAt<T>(col + 1, row),
            GetDotAt<T>(col - 1, row),
        };

        List<T> diagonals = new()
        {
            GetDotAt<T>(col + 1, row + 1),
            GetDotAt<T>(col + 1, row - 1),
            GetDotAt<T>(col - 1, row + 1),
            GetDotAt<T>(col - 1, row -1),
        };

        if (includesDiagonals)
        {
            neighbors.AddRange(diagonals);
        }
        return neighbors.Where((neighbor) => neighbor != null).ToList();
    }
    /// <summary>
    /// Returns a list of neighboring dots that surrounds
    /// an element at the given column and row
    /// </summary>
    /// <param name="col">The column of the board element whose neighbors are being found</param>
    /// <param name="row">The row of the board element whose neighbors are being found </param>
    /// <param name="includesDiagonals">Whether or not the method should return diagonal neighbors as well
    /// <returns>A list of the neighboring board elements </returns>
    public List<Dot> GetDotNeighbors(int col, int row, bool includesDiagonals = true)
    {

        List<Dot> neighbors = new()
        {
            GetDotAt(col, row + 1),
            GetDotAt(col, row - 1),
            GetDotAt(col + 1, row),
            GetDotAt(col - 1, row),
        };

        List<Dot> diagonals = new()
        {
            GetDotAt(col + 1, row + 1),
            GetDotAt(col + 1, row - 1),
            GetDotAt(col - 1, row + 1),
            GetDotAt(col - 1, row -1),
        };

        if (includesDiagonals)
        {
            neighbors.AddRange(diagonals);
        }
        return neighbors.Where((neighbor) => neighbor != null).ToList();
    }

    /// <summary>
    /// Returns a list of neighboring dots that surrounds
    /// an element at the given column and row
    /// </summary>
    /// <param name="col">The column of the board element whose neighbors are being found</param>
    /// <param name="row">The row of the board element whose neighbors are being found </param>
    /// <param name="includesDiagonals">Whether or not the method should return diagonal neighbors as well.
    /// Default is true
    /// <returns>A list of the neighboring board elements </returns>
    public List<Dot> GetClearedDotNeighbors(int col, int row, bool includesDiagonals = true)
    {

        List<Dot> neighbors = new()
        {
            GetClearedDotAt(col, row + 1),
            GetClearedDotAt(col, row - 1),
            GetClearedDotAt(col + 1, row),
            GetClearedDotAt(col - 1, row),
        };

        List<Dot> diagonals = new()
        {
            GetClearedDotAt(col + 1, row + 1),
            GetClearedDotAt(col + 1, row - 1),
            GetClearedDotAt(col - 1, row + 1),
            GetClearedDotAt(col - 1, row -1),
        };

        if (includesDiagonals)
        {
            neighbors.AddRange(diagonals);
        }
        return neighbors.Where((neighbor) => neighbor != null).ToList();
    }

    /// <summary>
    /// Returns a list of neighboring tiles that surrounds
    /// an element at the given column and row
    /// </summary>
    /// <typeparam name="T">A reference</typeparam>
    /// <param name="col">The column of the board element whose neighbors are being found</param>
    /// <param name="row">The row of the board element whose neighbors are being found </param>
    /// <param name="includesDiagonals">Whether or not the method should return diagonal neighbors as well
    /// <returns>A list of the neighboring board elements </returns>
    public List<T> GetTileNeighbors<T>(int col, int row, bool includesDiagonals = true)
        where T : class
    {

        List<T> neighbors = new()
        {
            GetTileAt<T>(col, row + 1),
            GetTileAt<T>(col, row - 1),
            GetTileAt<T>(col + 1, row),
            GetTileAt<T>(col - 1, row),
        };

        List<T> diagonals = new()
        {
            GetTileAt<T>(col + 1, row + 1),
            GetTileAt<T>(col + 1, row - 1),
            GetTileAt<T>(col - 1, row + 1),
            GetTileAt<T>(col - 1, row -1),
        };

        if (includesDiagonals)
        {
            neighbors.AddRange(diagonals);
        }
        return neighbors.Where((neighbor) => neighbor != null).ToList();
    }
    /// <summary>
    /// Returns a list of neighboring tiles that surrounds
    /// an element at the given column and row
    /// </summary>
    /// <param name="col">The column of the board element whose neighbors are being found</param>
    /// <param name="row">The row of the board element whose neighbors are being found </param>
    /// <param name="includesDiagonals">Whether or not the method should return diagonal neighbors as well
    /// <returns>A list of the neighboring board elements </returns>
    public List<Tile> GetTileNeighbors(int col, int row, bool includesDiagonals = true)
    {

        List<Tile> neighbors = new()
        {
            GetTileAt(col, row + 1),
            GetTileAt(col, row - 1),
            GetTileAt(col + 1, row),
            GetTileAt(col - 1, row),
        };

        List<Tile> diagonals = new()
        {
            GetTileAt(col + 1, row + 1),
            GetTileAt(col + 1, row - 1),
            GetTileAt(col - 1, row + 1),
            GetTileAt(col - 1, row -1),
        };

        if (includesDiagonals)
        {
            neighbors.AddRange(diagonals);
        }
        return neighbors.Where((neighbor) => neighbor != null).ToList();
    }
    

    /// <summary>
    /// Returns a board element at the given column and row
    /// </summary>
    /// <typeparam name="T">A Board Element</typeparam>
    /// <param name="col">The colummn of the board element</param>
    /// <param name="row">The row of the desired board element</param>
    /// <returns>The board element at the specified position</returns>
    public T GetDotAt<T>(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            if (Dots[col, row] is T t)
                return t;

            

        }
        return default;
    }

    public Dot GetDotAt(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
             return Dots[col, row];
        }
        return null;
    }
    
    
    public Dot GetClearedDotAt(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            return ClearedDots.FirstOrDefault((dot) => dot.Column == col && dot.Row == row);
        }
        return null;
    }

    public T GetTileAt<T>(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            if (Tiles[col, row] is T t)
                return t;
        }
        return default;
    }
    public Tile GetTileAt(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            return Tiles[col, row];
        }
        return null;
    }
    /// <summary>
    /// Gets a board mechanic tile at the specified column and row
    /// </summary>
    /// <typeparam name="T">The desired type that the desired board mechanic inherits</typeparam>
    /// <param name="col">The column of the desired board mechanic tile</param>
    /// <param name="row">The row of the desired board mechanic tile</param>
    /// <returns>A board mechanic tile at the specified column and row or null if not found</returns>
    public T GetBoardMechanicTileAt<T>(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            Tile tile = Tiles[col, row];
            if (tile is T t && tile.TileType.IsBoardMechanicTile())
                return t;
        }
        return default;
    }

    public void Put<T>(T dotsObject, int col, int row)
        where T : DotsGameObject
    {
        if (dotsObject is Dot dot)
            Dots[col, row] = dot;

        if (dotsObject is Tile tile)
        {
            Tiles[col, row] = tile;
        }

        
    }
    



    public bool FillBoard(DotsGameObjectData[] dotsToSpawn = null)
    {

        bool dotsDropped = false;
        for (int col = 0; col < Width; col++)
        {
            for (int row = Height - 1; row >= 0; row--)
            {
                Tile tile = Tiles[col, row];
                if (tile && tile.TileType.IsBlockable() )
                {
                    break;
                }
                if (!Dots[col, row]){
                    if(tile == null || !tile.TileType.IsBoardMechanicTile())
                    {
                        dotsDropped = true;

                        Dot dot = InitRandomDot(col, row, dotsToSpawn ?? this.dotsToSpawn);
                        DropDot(dot, row);

                    }
                }
                
            }
        }


        return dotsDropped;
    }


    public bool CollapseColumn()
    {
        bool dotsDropped = false;
        for (int col = 0; col < Width; col++)
        {

            for (int row = Height - 1; row >= 0; row--)
            {
                Tile tile = Tiles[col, row];
                if (tile && tile.TileType.IsBlockable())
                {
                    break;

                }
                if (!Dots[col, row])
                {
                    if(tile == null || !tile.TileType.IsBoardMechanicTile()){
                        for (int k = row + 1; k < Height; k++)
                        {
                            if (Dots[col, k] != null)
                            {
                                Dot dot = Dots[col, k];

                                Dots[col, row] = dot;
                                Dots[col, k] = null;
                                DropDot(dot, row);
                                dotsDropped = true;
                                break;
                            }

                        }
                    }

                    
                }


            }


        }
        return dotsDropped;
    }


    public bool IsOnEdgeOfBoard(int col, int row)
    {
        return IsAtBottomOfBoard(col, row) || IsAtLeftOfBoard(col, row) || IsAtRightOfBoard(col, row) || IsAtTopOfBoard(col, row);
    }
    /// <summary>
    /// Checks if the dot is at the far bottom of the board meaning no dots are under it.
    /// </summary>
    /// <param name="col"> the column of the dot to check</param>
    /// <param name="row">the row of the dot to check</param>
    /// <returns>Whether or not the dot is at the far bottom</returns>
    public bool IsAtBottomOfBoard(int col, int row)
    {

        for (int i = row - 1; i >= 0; i--)
        {
            Dot dot = GetDotAt(col, i);
            if (dot != null)
            {
                return false;
            }
        }

        return true;

    }

    /// <summary>
    /// Checks if the dot is on the far left of the board meaning no dots are to the left of it. 
    /// </summary>
    /// <param name="col"> the column of the dot to check</param>
    /// <param name="row">the row of the dot to check</param>
    /// <returns>Whether or not the dot is on the far left.</returns>
    public bool IsAtLeftOfBoard(int col, int row)
    {

        for (int i = col - 1; i >= 0; i--)
        {
            Dot dot = GetDotAt(i, row);

            if (dot != null)
            {
                return false;
            }
        }
        return true;

    }

    /// <summary>
    /// Checks if the dot is on the far right of the board meaning no dots are to the right of it. 
    /// </summary>
    /// <param name="col"> the column of the dot to check</param>
    /// <param name="row">the row of the dot to check</param>
    /// <returns>Whether or not the dot is on the far right.</returns>
    public bool IsAtRightOfBoard(int col, int row)
    {

        for (int i = col + 1; i < Width; i++)
        {
            Dot dot = GetDotAt(i, row);

            if (dot != null)
            {
                return false;
            }
        }

        return true;

    }

    /// <summary>
    /// Checks if the dot is on the very top of the board meaning no dots are above it. 
    /// </summary>
    /// <param name="col"> the column of the dot to check</param>
    /// <param name="row">the row of the dot to check</param>
    /// <returns>Whether or not the dot is on the very top.</returns>
    public bool IsAtTopOfBoard(int col, int row)
    {

        for (int i = row + 1; i < Height; i++)
        {
            Dot dot = GetDotAt(col, i);

            if (dot != null)
            {
                return false;
            }
        }

        return true;

    }
    public Dot GetBottomMostDot(int col)
    {
        for (int row = 0; row < Height; row++)
        {
            Dot dot = GetDotAt(col, row);
            if (dot)
            {
                return dot;
            }
        }
        return null;
    }


    public override string ToString()
    {

        string str = "";

        foreach (Dot dot in Dots)
        {
            if (dot)
            {
                str += dot.name + " ";
            }
            else
            {
                str += " null ";
            }
        }

        return str;
    }

    
    public bool Contains<T>()
        where T : DotsGameObject, IBoardElement
    {
        return FindElementsOfType<T>().Count > 0;
    }

   

    public List<T>  GetDotsGameObjectAt<T>(int col, int row)
    {
        List<T> dotsGameObjects = new();
        List<DotsGameObject> allDotsGameObjects = new();
        allDotsGameObjects.AddRange(GetDots());
        allDotsGameObjects.Concat(GetTiles());
        
        foreach(DotsGameObject dotsGameObject in allDotsGameObjects)
        {
            if(dotsGameObject.Column == col && dotsGameObject.Row == row && dotsGameObject is T t)
            {
                dotsGameObjects.Add(t);
            }
        }
       
        return dotsGameObjects;
        
       
    }

    public List<DotsGameObject> GetDotsGameObjectAt(int col, int row)
    {
        List<DotsGameObject> dotsGameObjects = new();
        List<DotsGameObject> allDotsGameObjects = new();
        allDotsGameObjects.AddRange(GetDots());
        allDotsGameObjects.AddRange(GetTiles());
        
        foreach(DotsGameObject dotsGameObject in allDotsGameObjects)
        {
            if(dotsGameObject.Column == col && dotsGameObject.Row == row && dotsGameObject)
            {
                dotsGameObjects.Add(dotsGameObject);
            }
        }
       
        return dotsGameObjects;
        
       
    }

    public List<T> GetBoardMechanicTileNeighbors<T>(int col, int row, bool includesDiagonals)
    {
        List<T> neighbors = new()
        {
            GetBoardMechanicTileAt<T>(col, row + 1),
            GetBoardMechanicTileAt<T>(col, row - 1),
            GetBoardMechanicTileAt<T>(col + 1, row),
            GetBoardMechanicTileAt<T>(col - 1, row),
        };

        List<T> diagonals = new()
        {
            GetBoardMechanicTileAt<T>(col + 1, row + 1),
            GetBoardMechanicTileAt<T>(col + 1, row - 1),
            GetBoardMechanicTileAt<T>(col - 1, row + 1),
            GetBoardMechanicTileAt<T>(col - 1, row -1),
        };

        if (includesDiagonals)
        {
            neighbors.AddRange(diagonals);
        }
        return neighbors.Where(neighbor => neighbor != null).ToList();
    }

    public List<T> FindTilesOfType<T>()
        where T : class
    {

        return Tiles.OfType<T>().ToList();
    }

    public List<T> FindDotsOfType<T>()
        where T : class
    {

        return Dots.OfType<T>().ToList();
    }
    public List<Dot> GetDots()
    {
        return Dots.OfType<Dot>().Where(dot => dot != null).ToList();
    }
    public List<Tile> GetTiles()
    {
        return Tiles.OfType<Tile>().Where(tile => tile != null).ToList();
    }
    public List<T> FindElementsOfType<T>()
        where T : class

    {
        List<T> tiles = Tiles.OfType<T>().ToList();
        List<T> dots = Dots.OfType<T>().ToList();
        return dots.Concat(tiles).ToList();
    }

    public List<T> FindBoardMechanicTilesOfType<T>()
    {
        List<T> tiles = Tiles
            .OfType<Tile>()
            .Where(tile => tile.TileType.IsBoardMechanicTile())
            .OfType<T>()
            .ToList();
        return tiles;
    }

    public List<T> FindGroupOfType<T>(T start, bool isValidNeighbor = true) where T : DotsGameObject{
       
       

        // Set to keep track of visited objects to avoid revisiting them
        HashSet<T> visited = new();

        // Queue for breadth-first search traversal
        Queue<T> queue = new();
        
        List<T> neighbors = new();
        
        List<T> group = new();

        // Enqueue the starting dot
        queue.Enqueue(start);

        // Perform breadth-first search
        while (queue.Count > 0)
        {
            T current = queue.Dequeue();

            group.Add(start);

            visited.Add(current);

            // Set neighbors of the current dots game object
            if(typeof(T) == typeof(Dot)|| typeof(T).IsSubclassOf(typeof(Dot))){

                neighbors = GetDotNeighbors<T>(current.Column, current.Row, false);
            }
            else if(typeof(T) == typeof(Tile) || typeof(T).IsSubclassOf(typeof(Tile))){
                neighbors = GetTileNeighbors<T>(current.Column, current.Row, false);
            }
            foreach (T neighbor in neighbors)
            {                
                
                if (!visited.Contains(neighbor))
                {
                    if(!isValidNeighbor){
                        continue;
                    } 
                    else{
                        // Add the neighbor to the queue for further exploration
                        queue.Enqueue(neighbor);
                        group.Add(neighbor);                    
                    } 
                   

                }
                visited.Add(neighbor);
            }
        }

        return group;
    }

    public List<T> FindDotsInColumn<T>(int col)
    {
        List<T> dots = new();
        for(int row = 0; row < Height; row++){
            Dot dot = GetDotAt(col, row);
            if(dot is T t){
                dots.Add(t);
            }
        }
        return dots.Where(dot => dot != null).ToList();
    }

    public List<Dot> FindDotsInColumn(int col)
    {
        List<Dot> dots = new();
        for(int row = 0; row < Height; row++){
            Dot dot = GetDotAt(col, row);
            dots.Add(dot);
        }
        
        return dots.Where(dot => dot != null).ToList();
    }

    public List<T> FindDotsInRow<T>(int row)
    {
        List<T> dots = new();
        for(int col = 0; col < Width; col++){
            Dot dot = GetDotAt(col, row);
            if(dot is T t){
                dots.Add(t);
            }
        }
        return dots.Where(dot => dot != null).ToList();
    }

    public List<Dot> FindDotsInRow(int row)
    {
        List<Dot> dots = new();
        for(int col = 0; col < Width; col++){
            Dot dot = GetDotAt(col, row);
            dots.Add(dot);
            
        }
        return dots.Where(dot => dot != null).ToList();
    }

    public List<Tile> FindBoardMechanicTilesInRow(int row)
    {
        List<Tile> tiles = new();
        for(int col = 0; col < Width; col++){
            Tile tile = GetTileAt(col, row);
            if(tile.TileType.IsBoardMechanicTile()){
                tiles.Add(tile);
            }

            
        }
        return tiles.Where(tile => tile != null).ToList();
    }

    public List<Tile> FindBoardMechanicTilesInColumn(int col)
    {
        List<Tile> tiles = new();
        for(int row = 0; row < Height; row++){
            Tile tile = GetTileAt(col, row);
            tiles.Add(tile);
        }
        
        return tiles.Where(tile => tile != null).ToList();
    }

public List<T> FindBoardMechanicTilesInRow<T>(int row)
    {
        List<T> tiles = new();
        for(int col = 0; col < Width; col++){
            Tile tile = GetTileAt(col, row);
            if(tile && tile.TileType.IsBoardMechanicTile()){
                if(tile is T t)
                    tiles.Add(t);
            }

            
        }
        return tiles;
    }

    public List<T> FindBoardMechanicTilesInColumn<T>(int col)
    {
        List<T> tiles = new();
        for(int row = 0; row < Height; row++){
            Tile tile = GetTileAt(col, row);
            if(tile is T t)
                tiles.Add(t);
        }
        
        return tiles;
    }
}
