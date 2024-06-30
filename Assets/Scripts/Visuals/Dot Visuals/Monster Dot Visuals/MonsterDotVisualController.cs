using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MonsterDotVisualController : ColorableDotVisualController, INumerableVisualController, IPreviewable
{
    private MonsterDot dot;
    public MonsterDotVisuals Visuals { get; private set; }
    private readonly NumerableVisualControllerBase numerableVisualController = new();
    public override T GetGameObject<T>()
    {
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (MonsterDot)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<MonsterDotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
        numerableVisualController.Init(Visuals.numerableVisuals);
        
    }


    public void UpdateNumbers(int amount)
    {
         numerableVisualController.UpdateNumbers(amount);
    }

    public IEnumerator PreviewHit(Type.HitType hitType)
    {
        float scaleDuration = 0.4f;
        UpdateNumbers(dot.TempNumber);

        Visuals.numerableVisuals.digit1.transform.DOScale(Vector2.one * 1.3f, scaleDuration);
        Visuals.numerableVisuals.digit2.transform.DOScale(Vector2.one * 1.3f, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);
        Visuals.numerableVisuals.digit1.transform.DOScale(Vector2.one, scaleDuration);
        Visuals.numerableVisuals.digit2.transform.DOScale(Vector2.one, scaleDuration);

    }

    public IEnumerator PreviewClear()
    {
        yield break;
    }
}
