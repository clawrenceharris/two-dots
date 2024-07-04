using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class ColorableVisualController : ConnectableDotVisualController
{



    public override IEnumerator DoBombHit()
    {
        
        //set the color to the bomb color
        SetColor(ColorSchemeManager.CurrentColorScheme.bombLight);

        yield return new WaitForSeconds(HittableVisuals.bombHitDuration);

        //set the color back to the default color
        SetColor();
        yield return null;
    }
    

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.FromDotColor(GetGameObject<IColorable>().Color);

    }

}
