using UnityEngine;

public class BlockTileVisualController : HittableVisualController, ITileVisualController
{
    private BlockTile tile;
    private HittableVisuals visuals;

    public override T GetGameObject<T>() => tile as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (BlockTile)dotsGameObject;
        visuals = dotsGameObject.GetComponent<HittableVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
    }

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
    }
}