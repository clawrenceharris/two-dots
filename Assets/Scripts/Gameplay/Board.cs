using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;


public class Board : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Dot[,] Dots;

    public Tile[,] Tiles { get; private set; }

    private DotsGameObjectData[] tilesOnBoard;
    private DotsGameObjectData[] dotsToSpawn;
    private DotsGameObjectData[] dotsOnBoard;
    private LineManager lineManager;

    public static float offset = 2.2f;

    public static float DotDropSpeed { get; private set; } = 0.4f;

    public static event Action<Board> onBoardCreated;
    public static event Action onDotsDropped;


    public void Init(LevelData level)
    {

        Width = level.width;
        Height = level.height;
        dotsOnBoard = level.dotsOnBoard;
        dotsToSpawn = level.dotsToSpawn;
        Dots = new Dot[level.width, level.height];
        Tiles = new Tile[level.width, level.height];
        lineManager = new LineManager(this);
        tilesOnBoard = level.tilesOnBoard;
        SetUpBoard();


    }
    private void SetUpBoard()
    {
        InitTiles();
        InitDots();
        FillBoard();
        onBoardCreated?.Invoke(this);

    }

    public bool HasDot(DotType dotType)
    {
        foreach (Dot dot in Dots)
        {
            if (dot && dot.DotType == dotType)
            {
                return true;
            }
        }
        return false;
    }


    private void Start()
    {
        DotsObjectEvents.onCleared += OnCleared;

    }

    private void Update()
    {
        lineManager.UpdateLines();
    }



    private void OnCleared(DotsGameObject dotsGameObject)
    {

        if (dotsGameObject is Dot dot)
        {
            //replace the dot that is being cleared with its replacement dot
            Dot replacement = InitDotOnBoard(dot.ReplacementDot);
            
            Dots[dot.Column, dot.Row] = replacement;
            
        }
        if (dotsGameObject is Tile tile)
        {
            Tiles[tile.Column, tile.Row] = null;

        }

        StartCoroutine(ClearCo(dotsGameObject));

    }

    private IEnumerator ClearCo(DotsGameObject dotsGameObject)
    {
        float waitTime = dotsGameObject.VisualController.GetVisuals<IHittableVisuals>().ClearDuration; 
        yield return new WaitForSeconds(waitTime);
        

        DestroyDotsGameObject(dotsGameObject);

    }

    public void DestroyDotsGameObject(DotsGameObject dotsGameObject)
    {
        Destroy(dotsGameObject.gameObject);
    }

    
    private void InitDots()
    {
        for (int i = 0; i < dotsOnBoard.Length; i++)
        {
            Dot dot = InitDotOnBoard(dotsOnBoard[i]);
            DotsGameObjectController.DropDot(dot, dot.Row, DotDropSpeed);


        }
    }


    private void InitTiles()
    {
        for (int i = 0; i < tilesOnBoard.Length; i++)
        {
            InitTile(tilesOnBoard[i]);
        }
    }

    private void InitTile(DotsGameObjectData data)
    {
        Tile tile = DotFactory.CreateDotsGameObject<Tile>(data);
        tile.transform.position = new Vector2(data.col, data.row) * offset;
        tile.transform.parent = transform;
        tile.name = data.type + " (" + data.col +  ", " + data.row  + ")";

        Tiles[data.col, data.row] = tile;
        tile.Init(data.col, data.row);
    }

    public List<T> GetElements<T>()
        where T : IBoardElement
    {
        List<T> boardElements = new();
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                T element = Get<T>(i, j);
                if (element != null)
                {
                    boardElements.Add(element);
                }
            }
        }
        return boardElements;
    }

    public List<IExplodable> GetExplodables()
    {

        List<IExplodable> explodables = new();
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                IExplodable explodable = GetExplodableAt(i, j);
                if (explodable != null)
                {
                    explodables.Add(explodable);
                }
            }
        }

        return explodables;
    }


    public void CreateBomb(int col, int row)
    {
        BombDot bomb = Instantiate(GameAssets.Instance.Bomb);
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
    private Dot InitDotOnBoard(DotsGameObjectData data)
    {

        Dot dot = null;

        if (data != null)
        {
            dot = DotFactory.CreateDotsGameObject<Dot>(data);
            dot.transform.position = new Vector2(data.col, data.row) * offset;
            dot.transform.parent = transform;
            dot.name = data.type + " (" + data.col + ", " + data.col + ")";
            dot.Init(data.col, data.row);
            Dots[data.col, data.row] = dot;
        }

        return dot;

    }


    private Dot InitRandomDot(int col, int row)
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
    public IHittable GetHittableAt(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            if (Dots[col, row] is IHittable dot)
            {
                return dot;
            }
            else if (Tiles[col, row] is IHittable tile)
            {
                return tile;
            }
        }
        return null;
    }
    /// <summary>
    ///Adds neighboring board elements based on
    ///their positions relative to the given row and column
    /// </summary>
    /// <typeparam name="T">A Board Element</typeparam>
    /// <param name="col">The column of the board element whose neighbors are being found</param>
    /// <param name="row">The row of the board element whose neighbors are being found </param>
    /// <param name="includesDiagonals">Whether or not the method should return diagonal neighbors as well
    /// <returns>A list of the neighboring board elements </returns>
    public List<T> GetNeighbors<T>(int col, int row, bool includesDiagonals = true) where T : IBoardElement
    {

        List<T> neighbors = new()
        {
            Get<T>(col, row + 1),
            Get<T>(col, row - 1),
            Get<T>(col + 1, row),
            Get<T>(col - 1, row),
        };

        List<T> diagonals = new()
        {
            Get<T>(col + 1, row + 1),
            Get<T>(col + 1, row - 1),
            Get<T>(col - 1, row + 1),
            Get<T>(col - 1, row -1),
        };

        if (includesDiagonals)
        {
            neighbors.AddRange(diagonals);
        }
        return neighbors;
    }




    /// <summary>
    /// Returns a board element at the given column and row
    /// </summary>
    /// <typeparam name="T">A Board Element</typeparam>
    /// <param name="col">The colummn of the board element</param>
    /// <param name="row">The row of the desired board element</param>
    /// <returns>The board element at the specified position</returns>
    public T Get<T>(int col, int row) where T : IBoardElement
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            if (Dots[col, row] is T dot)
                return dot;

            if (Tiles[col, row] is T tile)
                return tile;

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




    public bool FillBoard()
    {
        bool dotsDropped = false;
        for (int col = 0; col < Width; col++)
        {
            for (int row = Height - 1; row >= 0; row--)
            {
                if (Tiles[col, row] && Tiles[col, row].TileType == TileType.BlockTile)
                {
                    break;
                }
                if (!Dots[col, row] && !Tiles[col, row])
                {
                    dotsDropped = true;

                    Dot dot = InitRandomDot(col, row);
                    DotsGameObjectController.DropDot(dot, row, DotDropSpeed);

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
                if (Tiles[col, row] && Tiles[col, row].TileType == TileType.BlockTile)
                {
                    break;

                }
                if (!Tiles[col, row] && !Dots[col, row])
                {

                    for (int k = row + 1; k < Height; k++)
                    {
                        if (Dots[col, k] != null)
                        {
                            Dot dot = Dots[col, k];

                            Dots[col, row] = dot;
                            Dots[col, k] = null;
                            DotsGameObjectController.DropDot(dot, row, DotDropSpeed);
                            dotsDropped = true;
                            // Update keepGoing based on the result of IsAtBottomOfBoard
                            break;
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
            Dot dot = Get<Dot>(col, i);
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
            Dot dot = Get<Dot>(i, row);

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
            Dot dot = Get<Dot>(i, row);

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
            Dot dot = Get<Dot>(col, i);

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
            Dot dot = Get<Dot>(col, row);
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
        Debug.Log(typeof(T) + " == " + typeof(Dot));
        if (typeof(T).IsSubclassOf(typeof(Dot)))
        {
            foreach (Dot dot in Dots)
            {
                if (dot is T)
                {
                    return true;
                }
            }
        }
        else if (typeof(T).IsSubclassOf(typeof(Tile)))
        {
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
}
