using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    private EmptyTile tile;

    private EmptyTileVisuals visuals;
    public EmptyTileVisualController()
    {
        Board.onBoardCreated += OnBoardCreated;
    }

    private void OnBoardCreated(Board board)
    {
        SetSprite(board);

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
        base.Init(dotsGameObject);
    }

    public override void SetInitialColor()
    {
        Color bgColor = ColorSchemeManager.CurrentColorScheme.backgroundColor;
        visuals.spriteRenderer.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.9f);
    }
    

    



    private Sprite GetSprite( bool hasRight, bool hasLeft, bool hasTop, bool hasBottom )
    {

        

        if (hasLeft && hasRight && hasTop && hasBottom) return visuals.FullSprite;
        if (hasLeft && hasRight && !hasTop && !hasBottom) return visuals.HorizontalEdgeSprite;
        if (hasTop && hasBottom && !hasLeft && !hasRight) return visuals.VerticalEdgeSprite;
        
        if (hasLeft && hasRight && hasBottom && !hasTop) return visuals.TopEdgeSprite;
        if (hasLeft && hasRight && hasTop && !hasBottom) return visuals.BottomEdgeSprite;
        
        if (!hasLeft && hasRight && hasBottom && hasTop) return visuals.LeftEdgeSprite;
        if (hasLeft && !hasRight && hasBottom && hasTop) return visuals.RightEdgeSprite;
        
        if (hasLeft && !hasRight && !hasTop && hasBottom) return visuals.TopRightCornerSprite;
        if (!hasLeft && hasRight && !hasTop && hasBottom) return visuals.TopLeftCornerSprite;
        if (hasLeft && !hasRight && hasTop && !hasBottom) return visuals.BottomRightCornerSprite;
        if (!hasLeft && hasRight && hasTop && !hasBottom) return visuals.BottomLeftCornerSprite;

        if (!hasLeft && hasRight && !hasTop && !hasBottom) return visuals.LeftEndSprite;
        if (hasLeft && !hasRight && !hasTop && !hasBottom) return visuals.RightEndSprite;
        if (!hasLeft && !hasRight && hasTop && !hasBottom) return visuals.BottomEndSprite;
        if (!hasLeft && !hasRight && !hasTop && hasBottom) return visuals.TopEndSprite;

        // Default to FullSprite if no other condition matches
        return visuals.IsolatedSprite;

    }


    

    public virtual void SetSprite(Board board)
    {
        List<EmptyTile> neighbors = board.GetTileNeighbors<EmptyTile>(tile.Column, tile.Row, false);
        foreach(EmptyTile neighbor in neighbors){
            //If this water tile has another water tile to the left of it
            if(tile.Column == neighbor.Column + 1){
                hasLeft = true;
            }

            //If this water tile has another water tile to the right of it
            if(tile.Column == neighbor.Column - 1){
                hasRight = true;
            }

            //If this water tile has another water tile below it
             if(tile.Row == neighbor.Row + 1){
                hasBottom = true;
            }

            //If this water tile has another water tile above it
             if(tile.Row == neighbor.Row - 1){
                hasTop = true;
            }

        }

        visuals.spriteRenderer.sprite = GetSprite(hasRight, hasLeft, hasTop, hasBottom );
           
    }

}
