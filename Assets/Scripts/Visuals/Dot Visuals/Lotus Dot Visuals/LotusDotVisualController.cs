using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class LotusDotVisualController : ColorableDotVisualController
{
    private LotusDot dot;
    private LotusDotVisuals visuals;

    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (LotusDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<LotusDotVisuals>();

        base.Init(dotsGameObject);
    }

    public override void SetInitialColor()
    {
        Color initialColor = ColorSchemeManager.FromDotColor(dot.Color);
        foreach (Transform child in dot.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = initialColor;
        }

        visuals.Layer3.color = ColorUtils.LightenColor(initialColor, 0.5f);

    }

    public override void SetColor(Color color)
    {
        foreach (Transform child in dot.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = color;
        }

    }
    
    public IEnumerator DoIdleAnimation()
    {
        
        yield return null;
       

    }
   

   
         public IEnumerator DoHitPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoClearPreviewAnimation()
    {
        yield break;
    }
}
