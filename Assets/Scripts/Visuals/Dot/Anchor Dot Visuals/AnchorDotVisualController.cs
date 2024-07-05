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
    }

    protected override void SetColor()
    {
        //do nothing
    }
}