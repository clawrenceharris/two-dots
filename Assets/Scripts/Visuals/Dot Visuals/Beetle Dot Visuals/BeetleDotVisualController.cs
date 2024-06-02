using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BeetleDotVisualController : ColorDotVisualController
{
    protected new BeetleDotVisuals Visuals;

    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BeetleDotVisuals>();
        base.Init(dot);
    }

    public override void SetColor()
    {
        foreach(Transform child in Dot.transform)
        {
            if(child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = ColorSchemeManager.FromDotColor(Dot.Color);
            }
        }
        base.SetColor();
    }

    
    public override IEnumerator Hit(Type.HitType hitType)
    {
        if(Dot.HitCount == 1)
        {
           yield return DestroyWings(Visuals.rightWing3, Visuals.leftWing3);



        }
        else if(Dot.HitCount == 2)
        {
            yield return DestroyWings(Visuals.rightWing2, Visuals.leftWing2);
        }
        
      yield return base.Hit(hitType);
    }


    private IEnumerator DestroyWings(GameObject leftWing, GameObject rightWing)
    {
        rightWing.transform.DOScale(Vector2.zero, 0.7f);
        leftWing.transform.DOScale(Vector2.zero, 0.7f);

        yield return null;
    }
}
