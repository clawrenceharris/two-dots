using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterVisualController : TileVisualController
{
    private Water tile;
    private WaterVisuals visuals;
    public override T GetGameObject<T>() => tile as T;

    private static List<SpriteRenderer> tempWaterSprites = new();
    public override T GetVisuals<T>() => visuals as T;
   

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Water)dotsGameObject;
        visuals = dotsGameObject.GetComponent<WaterVisuals>();
        base.Init(dotsGameObject);

    }

    protected override void SetUp()
    {
        base.SetUp();
        visuals.Water.enabled = false;

        Board.onBoardCreated += OnBoardCreated;
    }

    private void OnBoardCreated(Board board){
        UpdateHoleSprites(board);
        UpdateWaterSprite(board);
        visuals.Water.enabled = true;
        visuals.WaterExtension.SpriteRenderer.enabled = false;
    }

    private void UpdateWaterSprite( Board board){
        bool hasRight = false;
        bool hasLeft= false;
        bool hasTop = false;
        bool hasBottom =false;
        List<Water> neighbors = board.GetTileNeighbors<Water>(tile.Column, tile.Row, false);
        foreach(Water neighbor in neighbors){
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
        visuals.Water.sprite = GetWaterSprite(hasLeft, hasRight, hasTop, hasBottom);
    }

    public void UpdateHoleSprites(Board board){
        bool hasRight = false;
        bool hasLeft= false;
        bool hasTop = false;
        bool hasBottom =false;
        List<Water> neighbors = board.GetTileNeighbors<Water>(tile.Column, tile.Row, false);
        foreach(Water neighbor in neighbors){
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

        visuals.spriteRenderer.sprite = GetHoleSprite(hasLeft, hasRight, hasTop, hasBottom);

    }

    
     private Sprite GetWaterSprite(bool hasLeft, bool hasRight, bool hasTop, bool hasBottom)
    {
       
        if(!hasTop && !hasBottom && !hasLeft && !hasRight) return visuals.IsolatedWaterSprite;


        if (hasLeft && !hasRight && !hasTop && hasBottom) return visuals.TopRightWaterSprite;
        if (!hasLeft && hasRight && !hasTop && hasBottom) return visuals.TopLeftWaterSprite;
        if (hasLeft && !hasRight && hasTop && !hasBottom) return visuals.BottomRightWaterSprite;
        if (!hasLeft && hasRight && hasTop && !hasBottom) return visuals.BottomLeftWaterSprite;

        if (!hasLeft && hasRight && !hasTop && !hasBottom) return visuals.LeftEndWaterSprite;
        if (hasLeft && !hasRight && !hasTop && !hasBottom) return visuals.RightEndWaterSprite;
        if (!hasLeft && !hasRight && hasTop && !hasBottom) return visuals.BottomEndWaterSprite;
        if (!hasLeft && !hasRight && !hasTop && hasBottom) return visuals.TopEndWaterSprite;

        // Default to FullSprite if no other condition matches
        return visuals.FullWaterSprite;
    }


   
    private Sprite GetHoleSprite(bool hasLeft, bool hasRight, bool hasTop, bool hasBottom)
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

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = Color.white;
        visuals.Water.color = ColorSchemeManager.CurrentColorScheme.water;

    }

    public override void SetColor(Color color)
    {
        base.SetColor(color);
        visuals.Water.color = color;
    }
    
    public override void EnableSprites(){
        visuals.Water.enabled = true;
        visuals.spriteRenderer.enabled = true;
    }
    public IEnumerator FillWater(Water endTile, Board board){
        
        Vector2 startPos = tile.transform.position;
        Vector2 endPos = endTile.transform.position;
        float distance = Vector2.Distance(startPos, endPos);
        
        SpriteRenderer water = UnityEngine.Object.Instantiate(visuals.Water);
        tempWaterSprites.Add(water);
        float angle = Vector2.SignedAngle(Vector2.right, startPos - endPos);
        
        ConnectorLine waterExtension = water.GetComponentInChildren<ConnectorLine>();
        waterExtension.SpriteRenderer.enabled = false;

        water.sprite = visuals.TempWaterSprite;
        water.transform.SetPositionAndRotation(startPos, Quaternion.Euler(0,0,angle));
        water.transform.localScale = tile.transform.localScale;
        water.transform.SetParent(null);

        waterExtension.transform.SetParent(water.transform);
        waterExtension.SpriteRenderer.color = ColorSchemeManager.CurrentColorScheme.water;
        waterExtension.transform.localRotation = Quaternion.Euler(0, 0, 0);
        waterExtension.transform.localScale = new Vector2(0, 1);
        float moveDuration = 0.4f;
        // float scaleDuration = moveDuration / 2;
        
        // float timePerUnit = moveDuration / path.Count;        
        water.enabled = true;
        waterExtension.SpriteRenderer.enabled = true;
        //    (water, path, moveDuration, (pos)=>{
            // Water waterTile = board.GetTileAt<Water>(pos.x, pos.y);
            // onMoveStepComplete?.Invoke(waterTile);

            // UpdateWaterSprites(tile, board);
            // visuals.Water.enabled =true;
  
        // }));
        CoroutineHandler.StartStaticCoroutine(MoveWater(water, endTile, moveDuration));
        waterExtension.transform.DOScale(Vector2.one, moveDuration);

        yield return new WaitForSeconds(moveDuration);

        
       
        
        
    }

    public void UpdateWaterSprites(Board board){
        UpdateWaterSprite(board);
        EnableSprites();
    }

    private IEnumerator MoveWater(SpriteRenderer water, Water endTile, float moveDuration){
        yield return water.transform.DOMove(endTile.transform.position, moveDuration);

    }

    public static void DestroyTempWaterSprites()
    {
        foreach(SpriteRenderer sprite in tempWaterSprites){
            UnityEngine.Object.Destroy(sprite.gameObject);
        }
        tempWaterSprites.Clear();
    }
}
