public class AnchorDotVisualController : DotVisualController
{
    private AnchorDot Dot;
    private DotVisuals Visuals;

    public override T GetGameObject<T>()
    {
        return Dot as T;
    }

    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        Dot = (AnchorDot)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<DotVisuals>();
    }

    protected override void SetColor()
    {
        //do nothing
    }
}