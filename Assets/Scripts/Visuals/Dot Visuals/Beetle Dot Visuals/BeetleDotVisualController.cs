using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BeetleDotVisualController : ColorDotVisualController
{
    protected new BeetleDotVisuals Visuals;
    protected new BeetleDot Dot;
    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BeetleDotVisuals>();
        Dot = dot.GetComponent<BeetleDot>();
        base.Init(Dot);
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


    private IEnumerator DoHitAnimation()
    {
        if (Dot.HitCount == 1)
        {
            yield return RemoveWings(Visuals.rightWing3, Visuals.leftWing3);



        }
        else if (Dot.HitCount == 2)
        {
            yield return RemoveWings(Visuals.rightWing2, Visuals.leftWing2);
        }

    }
    public IEnumerator Rotate()
    {
        Vector3 rotation = Vector3.zero;
        if(Dot.DirectionY < 0)
        {
            rotation = new Vector3(0, 0, 180);
        }

        if(Dot.DirectionX < 0)
        {
            rotation = new Vector3(0, 0, -90);

        }
        if (Dot.DirectionX > 0)
        {
            rotation = new Vector3(0, 0, 90);

        }

        yield return Dot.transform.DORotate(rotation, 0.5f);
    }
    
    public override IEnumerator Hit(Type.HitType hitType)
    {
        yield return DoHitAnimation();
        yield return base.Hit(hitType);
    }


    private IEnumerator RemoveWings(GameObject leftWing, GameObject rightWing)
    {
        rightWing.transform.DOScale(Vector2.zero, 0.7f);
        leftWing.transform.DOScale(Vector2.zero, 0.7f);

        yield return null;
    }
}
