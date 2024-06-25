using UnityEngine;

public class BlockTileVisualController : HittableVisualController, ITileVisualController
{
    private BlockTile tile;
    private TileVisuals visuals;

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
        tile = (BlockTile)dotsGameObject;
        visuals = dotsGameObject.GetComponent<TileVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
    }

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
    }
}