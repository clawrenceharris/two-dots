using UnityEngine;

public class EmptyTileVisualController : VisualController, ITileVisualController
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

    private EmptyTile Tile;

    private EmptyTileVisuals Visuals;
    private Board board;
    public EmptyTileVisualController()
    {
        Board.onBoardCreated += OnBoardCreated;
    }

    private void OnBoardCreated(Board board)
    {
        this.board = board;
        SetUp(board);
        InitSprite();

    }

    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }

    public override T GetGameObject<T>()
    {
       return Tile as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        Tile = (EmptyTile)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<EmptyTileVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        SetUp();
    }

    protected override void SetColor()
    {
        Color bgColor = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        spriteRenderer.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.5f);
    }
    private void SetUp(Board board)
    {
        
            
        if (board.Get<Tile>(Tile.Column, Tile.Row + 1))
        {

            hasTop = true;
        }

        if (board.Get<Tile>(Tile.Column, Tile.Row - 1))
        {

            hasBottom = true;
        }


        if (board.Get<Tile>(Tile.Column + 1, Tile.Row ))
                
        {

            hasRight = true;
        }
        if (board.Get<Tile>(Tile.Column - 1, Tile.Row))

        {
            hasLeft = true;
        }

        if (board.IsAtLeftOfBoard(Tile.Column, Tile.Row))
        {
            isLeft = true;
        }

        if(board.IsAtTopOfBoard(Tile.Column, Tile.Row))
        {
            isTop = true;
        }

        if(board.IsAtBottomOfBoard(Tile.Column, Tile.Row))
        {
            isBottom = true;
        }
        if(board.IsAtRightOfBoard(Tile.Column, Tile.Row))
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

            return Visuals.LeftBottom;

        }
        if (IsBottom())
        {

            return Visuals.Bottom;

        }
        if (IsTop())
        {
            return Visuals.Top;
        }
        if (IsLeft())
        {
            return Visuals.Left;

        }
        if (IsRight())
        {
            return Visuals.Right;
        }
        if (IsRightBottom())
        {
            return Visuals.RightBottom;

        }
        if (IsRightTop())
        {
            return Visuals.RightTop;

        }
        if (IsLeftTop())
        {
            return Visuals.LeftTop;

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

        spriteRenderer.sprite = GetSprite();
            

        



    }

}
