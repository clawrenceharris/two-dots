 
using System.Collections;
using Animations;

public class PreviewableVisualController : IPreviewableVisualController{
    private VisualController visualController;

    public void Init(VisualController visualController){
        this.visualController = visualController;
    }
   
    public IEnumerator PreviewClear()
    {
        yield return visualController.Animate(new PreviewClearAnimation(),AnimationLayer.PreviewLayer);
    }

    
    public IEnumerator PreviewHit()
    {
        yield return visualController.Animate(new PreviewHitAnimation(),AnimationLayer.PreviewLayer);
    }

   
    public IEnumerator Idle()
    {
        yield return visualController.Animate(new IdleAnimation(),AnimationLayer.PreviewLayer);
    }

    
}
 
 
