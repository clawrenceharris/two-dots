using UnityEngine;

public class AnchorDotVisualController : DotVisualController
{
    private AnchorDot dot;
    private DotVisuals visuals;

    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (AnchorDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<DotVisuals>();
        base.Init(dotsGameObject);
    }

    public override void SetInitialColor()
    {
         visuals.spriteRenderer.sprite = visuals.sprite;
        
        
    }

    public override void SetColor(Color color)
    {
        visuals.spriteRenderer.sprite = visuals.bombHitSprite;
    }
}