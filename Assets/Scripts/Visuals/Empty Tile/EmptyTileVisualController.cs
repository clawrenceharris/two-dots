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

    private EmptyTile tile;

    private EmptyTileVisuals visuals;
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
        SetUp(board);
    }

    public override void SetInitialColor()
    {
        Color bgColor = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        visuals.spriteRenderer.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.5f);
    }
    private void SetUp(Board board)
    {
        
            
        if (board.GetTileAt(tile.Column, tile.Row + 1))
        {

            hasTop = true;
        }

        if (board.GetTileAt(tile.Column, tile.Row - 1))
        {

            hasBottom = true;
        }


        if (board.GetTileAt(tile.Column + 1, tile.Row ))
                
        {

            hasRight = true;
        }
        if (board.GetTileAt(tile.Column - 1, tile.Row))

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

    

    public virtual void InitSprite()
    {
        visuals.spriteRenderer.sprite = GetSprite();
           
    }

}
