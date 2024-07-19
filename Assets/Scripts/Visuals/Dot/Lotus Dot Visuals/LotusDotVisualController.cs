using System.Collections;
using System.Collections.Generic;
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

        visuals.middle.color = ColorUtils.LightenColor(initialColor, 0.5f);

        base.SetInitialColor();
    }

    public override void SetColor(Color color)
    {
        foreach (Transform child in dot.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = color;
        }

        base.SetColor(color);
    }



}
