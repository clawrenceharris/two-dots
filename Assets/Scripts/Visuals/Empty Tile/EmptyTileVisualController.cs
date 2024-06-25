using UnityEngine;

public class EmptyTileVisualController : TileVisualController
{
    private bool hasLeft;
    private bool hasRight;
    private bool hasTop;
    private bool hasBottom;
    private bool isBottom;
    private bool isTop;
    private bool isLeft;
    private bool isRight;
    private bool flipY;
    private bool flipX;
<<<<<<< Updated upstream
    
    private new EmptyTileVisuals Visuals;
=======

    private EmptyTile tile;

    private EmptyTileVisuals visuals;
>>>>>>> Stashed changes
    private Board board;
    public EmptyTileVisualController()
    {
        Board.onBoardCreated += OnBoardCreated;
    }

    public override void Init(Tile tile)
    {
        base.Init(tile);
        Visuals = tile.GetComponent<EmptyTileVisuals>();
        


    }
    private void OnBoardCreated(Board board)
    {
        this.board = board;
        SetUp(board);
        InitSprite();

    }
<<<<<<< Updated upstream
    
=======

    public override T GetGameObject<T>()
    {
       return tile as T;
    }

    public override T GetVisuals<T>()
    {
        return visuals as T;
    }
    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (EmptyTile)dotsGameObject;
        visuals = dotsGameObject.GetComponent<EmptyTileVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp(board);
    }

    protected override void SetColor()
    {
        Color bgColor = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        spriteRenderer.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.5f);
    }
>>>>>>> Stashed changes
    private void SetUp(Board board)
    {
        
            
<<<<<<< Updated upstream
        if (board.GetBoardElementDotAt<Tile>(Tile.Column, Tile.Row + 1))
=======
        if (board.Get<Tile>(tile.Column, tile.Row + 1))
>>>>>>> Stashed changes
        {

            hasTop = true;
        }

<<<<<<< Updated upstream
        if (board.GetBoardElementDotAt<Tile>(Tile.Column, Tile.Row - 1))
=======
        if (board.Get<Tile>(tile.Column, tile.Row - 1))
>>>>>>> Stashed changes
        {

            hasBottom = true;
        }


<<<<<<< Updated upstream
        if (board.GetBoardElementDotAt<Tile>(Tile.Column + 1, Tile.Row ))
=======
        if (board.Get<Tile>(tile.Column + 1, tile.Row ))
>>>>>>> Stashed changes
                
        {

            hasRight = true;
        }
<<<<<<< Updated upstream
        if (board.GetBoardElementDotAt<Tile>(Tile.Column - 1, Tile.Row))
=======
        if (board.Get<Tile>(tile.Column - 1, tile.Row))
>>>>>>> Stashed changes

        {
            hasLeft = true;
        }

        if (board.IsAtLeftOfBoard(tile.Column, tile.Row))
        {
            isLeft = true;
        }

        if(board.IsAtTopOfBoard(tile.Column, tile.Row))
        {
            isTop = true;
        }

        if(board.IsAtBottomOfBoard(tile.Column, tile.Row))
        {
            isBottom = true;
        }
        if(board.IsAtRightOfBoard(tile.Column, tile.Row))
        {
            isRight = true;
        }
       

    }

    //------ sprite logic-------------//
    #region
    private bool IsCenter()
    {
       
        return hasRight && hasLeft && hasTop && hasBottom;

    }
    private bool IsLeftBottom()
    {
        return hasLeft && hasBottom  && !hasRight;
    }
    private bool IsRightBottom()
    {
        return hasRight && hasBottom && !hasTop && !hasLeft;

    }
    private bool IsBottom()
    {
        return hasBottom && !hasRight && !hasLeft && !hasTop;
    }

    private bool IsTop()
    {
        return hasTop && !hasRight && !hasLeft && !hasBottom;
    }
    private bool IsRightTop()
    {
        return hasRight && hasTop && !hasBottom && !hasLeft;

    }
    private bool IsLeftTop()
    {
        return hasLeft && hasTop && !hasBottom && !hasRight;
    }
   

    private bool IsSingle()
    {
        return !hasLeft && !hasRight && !hasTop && !hasBottom;
    }

    

    private bool IsLeftTopBottom()
    {
        return hasLeft && hasBottom && hasTop && !hasRight;
    }
    private bool IsLeft()
    {
        return hasLeft && !hasBottom && !hasTop && !hasRight;
    }
    private bool IsRight()
    {
        return hasRight && !hasBottom && !hasTop && !hasLeft;
    }
    private bool IsRightTopBottom()
    {
        return hasRight && hasBottom && hasTop && !hasLeft;
    }

    private bool IsLeftRightTop()
    {
        return hasTop && !hasBottom && hasLeft && hasRight;
    }
    private bool IsLeftRightBottom()
    {
        return hasBottom && !hasTop && hasLeft && hasRight;
    }

    
    #endregion



    private Sprite GetSprite()
    {

        //if (IsCenter())
        //{
        //    return visuals.Center;
        //}

        if (IsLeftBottom())
        {

            return visuals.LeftBottom;

        }
        if (IsBottom())
        {

            return visuals.Bottom;

        }
        if (IsTop())
        {
            return visuals.Top;
        }
        if (IsLeft())
        {
            return visuals.Left;

        }
        if (IsRight())
        {
            return visuals.Right;
        }
        if (IsRightBottom())
        {
            return visuals.RightBottom;

        }
        if (IsRightTop())
        {
            return visuals.RightTop;

        }
        if (IsLeftTop())
        {
            return visuals.LeftTop;

        }




        //else if (IsSingle())
        //{
        //    return visuals.Single;


        //}

        //else if (IsLeftTopBottom())
        //{

        //    return visuals.LeftTopBottom;

        //}

        //else if (IsRightTopBottom())
        //{
        //    return visuals.RightTopBottom;

        //}

        //else if (IsLeftRightBottom())
        //{
        //    return visuals.LeftRightBottom;

        //}

        //else if (IsLeftRightTop())
        //{
        //    return visuals.LeftRightTop;

        //}


        else
        {
            return null;
            
        }

    }


    private bool ShouldFlipY()
    {
        if (isBottom)
        {
            return true;
        }
        return false;
    }

    private bool ShouldFlipX()
    {
        if (isRight &&  isBottom)
        {
            return true;
        }
        return false;
    }

    public virtual void InitSprite()
    {

        //SpriteRenderer.flipY = ShouldFlipY();

        SpriteRenderer.sprite = GetSprite();
            

        



    }
}
