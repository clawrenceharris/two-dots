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
        SetColor(ColorSchemeManager.FromDotColor(dot.Color));
    }

    public override void SetColor(Color color)
    {
        visuals.spriteRenderer.color = ColorUtils.LightenColor(color, 0.1f);;
        visuals.GemTopLeft.color = color;
        visuals.GemBottomRight.color = color;
        visuals.GemBottomLeft.color = ColorUtils.DarkenColor(color, 0.4f);
        visuals.GemLeft.color = ColorUtils.DarkenColor(color, 0.3f);
        visuals.GemTop.color = ColorUtils.LightenColor(color, 0.48f);
        visuals.GemRight.color = ColorUtils.LightenColor(color, 0.48f);
        visuals.GemTopRight.color = ColorUtils.LightenColor(color, 0.7f);
        visuals.GemBottom.color = ColorUtils.DarkenColor(color, 0.3f);
    }

}
