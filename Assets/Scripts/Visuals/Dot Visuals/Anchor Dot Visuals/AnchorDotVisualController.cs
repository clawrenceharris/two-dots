using UnityEngine;

public class AnchorDotVisualController : DotVisualController
{
    private AnchorDot dot;
    public DotVisuals Visuals { get; private set; }

    public override T GetGameObject<T>()
    {
        return dot as T;
    }

    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }
    public override void Init(DotsGameObject dotsGameObject)
    {
        base.Init(dotsGameObject);

        dot = (AnchorDot)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<DotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
    }

    protected override void SetColor()
    {
        //do nothing
    }
}