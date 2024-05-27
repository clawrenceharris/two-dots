using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;


public class Board : MonoBehaviour
{
    public int Width;
    public int Height;

    public Dot[,] Dots { get; private set; }

    public Tile[,] Tiles { get; private set; }

    private TileData[] tilesOnBoard;
    public static event Action OnWin;
    private DotToSpawnData[] dotsToSpawn;
    private DotData[] dotsOnBoard;
    private LineManager lineManager;

    public static float offset = 2.5f;
    public static float DotDropSpeed { get; private set; } = 0.5f;

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
        foreach(Dot dot in Dots)
        {
            if(dot && dot.DotType == dotType)
            {
                return true;
            }
        }
        return false;
    }


    private void Start()
    {
        Dot.onDotCleared += OnDotCleared;
        Tile.onTileCleared += OnTileCleared;

    }
     private void Update()
    {
        lineManager.UpdateLines();
    }

    private void OnDotCleared(Dot dot)
    {
        Dots[dot.Column, dot.Row] = null;
        DotController.DestroyDot(dot);
    }

    private void OnTileCleared(Tile tile)
    {
        Tiles[tile.Column, tile.Row] = null;
        TileController.DestroyTile(tile);
    }

    private void InitDots()
    {
        for (int i = 0; i < dotsOnBoard.Length; i++)
        {
            Dot dot = InitDotOnBoard(dotsOnBoard[i]);
            DotController.DropDot(dot, dot.Row, DotDropSpeed);


        }
    }


    private void InitTiles()
    {
        for (int i = 0; i < tilesOnBoard.Length; i++)
        {
            InitTile(tilesOnBoard[i]);
        }
    }

    private void InitTile(TileData tileData)
    {
        Tile tile = TileFactory.CreateTile(tileData);
        tile.Init(tileData.col, tileData.row);
        tile.transform.position = new Vector2(tileData.col, tileData.row) * offset;
        tile.transform.parent = transform;
        tile.name = "(" + tile.Column + ", " + tile.Row + ")";

        Tiles[tile.Column, tile.Row] = tile;
    }

    public List<IHittable> GetHittables()
    {
        List<IHittable> hittables = new();
        for (int i = 0; i < Width; i++)
        {
            for(int j = 0; j < Height; j++)
            {
                IHittable hittable = GetHittableAt(i, j);
                if (hittable != null)
                {
                    hittables.Add(hittable);
                }
            }
        }
        return hittables;
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
    public List<Dot> GetBombDots()
    {
        List<Dot> bombDots = new();
        foreach (Dot dot in Dots)
        {
            if(dot && dot.IsBomb)
            {
                bombDots.Add(dot);
            }
        }

        return bombDots;
    }
    public void CreateBomb(int col, int row)
    {

        Bomb bomb = Instantiate(GameAssets.Instance.Bomb);
        bomb.transform.position = new Vector2(col, row) * offset;
        bomb.transform.parent = transform;
        Dots[col, row] = bomb;
        bomb.Init(col, row);
        
    }
    public void PutDot(Dot dot)
    {
        Dots[dot.Column, dot.Row] = dot;
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
    private Dot InitDotOnBoard(DotData dotData)
    {

        Dot dot = DotFactory.CreateDot(dotData);
        dot.Init(dotData.col, dotData.row);
        dot.transform.parent = transform;
        dot.name = "(" + dot.Column + ", " + dot.Row + ")";
        dot.transform.position = new Vector2(dot.Column, dot.Row) * offset;

        Dots[dot.Column, dot.Row] = dot;
        return dot;

    }





    private Dot InitRandomDot(int col, int row)
    {

        int randDot = UnityEngine.Random.Range(0, dotsToSpawn.Length);

        DotToSpawnData dotData = dotsToSpawn[randDot];

        Dot dot = DotFactory.CreateDot(dotData);
        dot.Init(col, row);
        dot.transform.parent = transform;
        dot.name = "(" + col + ", " + row + ")";
        dot.transform.position = new Vector2(col, row + 100) * offset;

        Dots[col, row] = dot;
        return dot;



    }

    public void RemoveDot(Dot dot)
    {
        Dots[dot.Column, dot.Row] = null;
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
    public List<IHittable> GetHittableNeighbors(int col, int row, bool diagonal = false)
    {
        List<IHittable> neighbors = new()
        {
            //Add neighboring dots based on their positions relative to the given row and column
            GetHittableAt(col, row + 1),
            GetHittableAt(col, row - 1),
            GetHittableAt(col + 1, row),
            GetHittableAt(col - 1, row),

        };
        
        List<IHittable> diagonals = new()
        {
            GetHittableAt(col + 1 , row + 1),
            GetHittableAt(col - 1, row - 1),
            GetHittableAt(col + 1, row - 1),
            GetHittableAt(col -1, row + 1),
        };
        if (diagonal)
        {
            neighbors.AddRange(diagonals);

        }
        return neighbors;
    }

    public List<Dot> GetDotNeighbors(int col, int row, bool diagonal = false)
    {

        List<Dot> neighbors = new()
        {
            // Add neighboring dots based on their positions relative to the given row and column
            GetDotAt(col, row + 1),
            GetDotAt(col, row - 1),
            GetDotAt(col + 1, row),
            GetDotAt(col - 1, row),
        };

        List<Dot> diagonals = new()
        {
            GetDotAt(col +1 , row + 1),
            GetDotAt(col -1, row + 1),
            GetDotAt(col - 1, row - 1),
            GetDotAt(col + 1, row - 1),
        };
        if (diagonal)
        {
            neighbors.AddRange(diagonals);

        }
        return neighbors;
    }

    public List<Tile> GetTileNeighbors(int col, int row, bool diagonal )
    {

        List<Tile> neighbors = new()
        {
            // Add neighboring dots based on their positions relative to the given row and column
            GetTileAt(col, row + 1),
            GetTileAt(col, row - 1),
            GetTileAt(col + 1, row),
            GetTileAt(col - 1, row),
        };

        List<Tile> diagonals = new()
        {
            GetTileAt(col +1 , row + 1),
            GetTileAt(col -1, row + 1),
            GetTileAt(col - 1, row - 1),
            GetTileAt(col + 1, row - 1),
        };
        if (diagonal)
        {
            neighbors.AddRange(diagonals);

        }
        return neighbors;
    }


    

    public Dot GetDotAt(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            return Dots[col, row];
        }
        return null;
    }
    public ConnectableDot GetConnectableDotAt(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            Dot dot = Dots[col, row];
            if (dot is ConnectableDot cDot)
                return cDot;
        }
        return null;
    }

    public Tile GetTileAt(int col, int row)
    {
        if (col >= 0 && col < Width && row >= 0 && row < Height)
        {
            return Tiles[col, row];
        }
        return null;
    }

    public Tile GetTileAt(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height)
        {
            return Tiles[pos.x, pos.y];
        }
        return null;
    }
    public Dot GetDotAt(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height)
        {
            return Dots[pos.x, pos.y];
        }
        return null;
    }



    public void MoveDot(Dot dotToMove, int desinationCol, int destinationRow)
    {
        
        Dots[desinationCol, destinationRow] = dotToMove;

        
        Dots[dotToMove.Column, dotToMove.Row] = null;

        dotToMove.Column = desinationCol;
        dotToMove.Row = destinationRow;

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
                    DotController.DropDot(dot, row, DotDropSpeed);

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
                            DotController.DropDot(dot, row, DotDropSpeed);
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

        foreach(Dot dot in Dots)
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

    internal void MoveTile(int column, int row1, int col, int row2)
    {
        throw new NotImplementedException();
    }

    internal void MoveTile(Tile tile, int col, int row)
    {
        throw new NotImplementedException();
    }
}
