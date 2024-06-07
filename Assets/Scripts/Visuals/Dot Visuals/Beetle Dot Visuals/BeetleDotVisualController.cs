using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;

public class BeetleDotVisualController : ColorDotVisualController
{
    protected new BeetleDotVisuals Visuals;
    protected new BeetleDot Dot;
    private List<GameObject> wings;
    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BeetleDotVisuals>();
        Dot = dot.GetComponent<BeetleDot>();
        base.Init(Dot);
    }

    protected override void SetUp()
    {
        wings = new() {
            Visuals.leftWing1,
            Visuals.leftWing2,
            Visuals.leftWing3,
            Visuals.rightWing1,
            Visuals.rightWing2,
            Visuals.rightWing3
        };
        Rotate();
        base.SetUp();
    }

    protected override void SetColor()
    {
        
        foreach(GameObject wing in wings)
        {
            if(wing.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = ColorSchemeManager.FromDotColor(Dot.Color);
            }
        }
           
        
    }

    public override void DisableSprites()
    {
        foreach (GameObject wing in wings)
        {
            if (wing.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = false;
            }
        }

        base.DisableSprites();
    }
    public override void EnableSprites()
    {
        foreach (GameObject wing in wings)
        {
            if (wing.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = true;
            }
        }


        base.EnableSprites();
    }

    private IEnumerator DoHitAnimation()
    {
        Dot.StopAllCoroutines();
        Visuals.leftWings.DOLocalRotate(Vector3.zero, 0.1f);
        Visuals.rightWings.DOLocalRotate(Vector3.zero, 0.1f);
        if (Dot.HitCount == 1)
        {
            yield return RemoveWings(Visuals.rightWing3, Visuals.leftWing3);
            Visuals.rightWing2.transform.parent = Visuals.rightWings;
            Visuals.leftWing2.transform.parent = Visuals.leftWings;

        }
        else if (Dot.HitCount == 2)
        {
            yield return RemoveWings(Visuals.rightWing2, Visuals.leftWing2);
            Visuals.rightWing1.transform.parent = Visuals.rightWings;
            Visuals.leftWing1.transform.parent = Visuals.leftWings;

        }

        else
        {
            yield return Dot.transform.DOMove(Dot.transform.position * 2, 1f);
        }
          

    }

    public override IEnumerator PreviewHit(HitType hitType)
    {
        float flapDuration = 0.15f;
        float flapAngle = 45f;

        Vector3 leftWingAngle = new(0, 0, -flapAngle);
        Vector3 rightWingAngle = new(0, 0, flapAngle);


        while (ConnectionManager.ToHit.Contains(Dot))
        {
            // Flap up
            Visuals.leftWings.DOLocalRotate(leftWingAngle, flapDuration);
            Visuals.rightWings.DOLocalRotate(rightWingAngle, flapDuration);

            yield return new WaitForSeconds(flapDuration);

            // Flap down
            Visuals.leftWings.DOLocalRotate(Vector3.zero, flapDuration);
            Visuals.rightWings.DOLocalRotate(Vector3.zero, flapDuration);
            yield return new WaitForSeconds(flapDuration);

        }
        Visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

        yield return base.PreviewHit(hitType);
    }

    

    public IEnumerator RotateCo()
    {

        Vector3 rotation = GetRotation();
        yield return Dot.transform.DOLocalRotate(rotation, 0.5f);
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

        

        Dot.transform.localRotation = Quaternion.Euler(rotation);
    }

    public override IEnumerator Hit(HitType hitType)
    {
        yield return DoHitAnimation();
        yield return base.Hit(hitType);
    }


    private IEnumerator RemoveWings(GameObject leftWing, GameObject rightWing)
    {

        rightWing.transform.SetParent(null);
        leftWing.transform.SetParent(null);
        rightWing.transform.DOScale(Vector2.zero, 0.7f);
        leftWing.transform.DOScale(Vector2.zero, 0.7f);
        yield return new WaitForSeconds(0.7f);
        Object.Destroy(leftWing);
        Object.Destroy(rightWing);
        wings.Remove(leftWing);
        wings.Remove(rightWing);




    }
}
