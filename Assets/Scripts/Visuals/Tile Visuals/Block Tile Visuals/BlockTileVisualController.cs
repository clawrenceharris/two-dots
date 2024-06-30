using UnityEngine;

public class BlockTileVisualController : HittableVisualController, ITileVisualController
{
    private BlockTile tile;
    public HittableVisuals Visuals { get; private set; }

    public override T GetGameObject<T>()
    {
        return tile as T;
    }


    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        base.Init(dotsGameObject);

        tile = (BlockTile)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<HittableVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
    }

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
    }
}