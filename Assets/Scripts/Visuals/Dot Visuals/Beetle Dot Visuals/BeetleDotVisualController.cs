using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
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

    protected override void SetUp()
    {
        Rotate();
        base.SetUp();
    }

    protected override void SetColor()
    {
        foreach(Transform child in Visuals.leftWings.transform)
        {
            if(child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = ColorSchemeManager.FromDotColor(Dot.Color);
            }
        }
        foreach (Transform child in Visuals.rightWings.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
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

    public override IEnumerator PreviewHit(HitType hitType)
    {
        while(Dot.HitType == HitType.Connection)
        {
            Visuals.leftWings.DORotate(new Vector3(0, 0, 90), 1f).OnComplete(() =>
            {
                Visuals.leftWings.DORotate(new Vector3(0, 0, 0), 1f);
            });
            yield return Visuals.rightWings.DORotate(new Vector3(0, 0, -90), 1f).OnComplete(() =>
            {
                Visuals.rightWings.DORotate(new Vector3(0, 0, 0), 1f);
            });
            
        }
        yield return base.PreviewHit(hitType);
    }

    public IEnumerator RotateCo()
    {

        Vector3 rotation = GetRotation();
        yield return Dot.transform.DORotate(rotation, 0.5f);
    }

    private Vector3 GetRotation()
    {
        Vector3 rotation = Vector3.zero;
        if (Dot.DirectionY < 0)
        {
            rotation = new Vector3(0, 0, 180);
        }

        if (Dot.DirectionX < 0)
        {
            rotation = new Vector3(0, 0, 90);

        }
        if (Dot.DirectionX > 0)
        {
            rotation = new Vector3(0, 0, -90);

        }
        return rotation;
    }

    private void Rotate()
    {
        Vector3 rotation = GetRotation();

        

        Dot.transform.DORotate(rotation, 0.5f);
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
