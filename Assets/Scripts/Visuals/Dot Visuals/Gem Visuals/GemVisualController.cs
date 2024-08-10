using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemVisualController : ColorableDotVisualController, IPreviewableVisualController
{
    private Gem dot;

    private GemVisuals visuals;

    public IEnumerator DoClearPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoHitPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoIdleAnimation()
    {
        yield break;
    }

    public override T GetGameObject<T>() => dot as T;
   
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (Gem)dotsGameObject;
        visuals = dotsGameObject.GetComponent<GemVisuals>();   

        base.Init(dotsGameObject);
    }

    public override void SetInitialColor()
    {
        base.SetInitialColor();
        Color baseColor = ColorSchemeManager.FromDotColor(dot.Color);
        visuals.spriteRenderer.color = ColorUtils.LightenColor(baseColor, 0.1f);;
        visuals.GemTopLeft.color = baseColor;
        visuals.GemBottomRight.color = baseColor;
        visuals.GemBottomLeft.color = ColorUtils.DarkenColor(baseColor, 0.4f);
        visuals.GemLeft.color = ColorUtils.DarkenColor(baseColor, 0.3f);
        visuals.GemTop.color = ColorUtils.LightenColor(baseColor, 0.48f);
        visuals.GemRight.color = ColorUtils.LightenColor(baseColor, 0.48f);
        visuals.GemTopRight.color = ColorUtils.LightenColor(baseColor, 0.7f);
        visuals.GemBottom.color = ColorUtils.DarkenColor(baseColor, 0.3f);
    }
   
}
