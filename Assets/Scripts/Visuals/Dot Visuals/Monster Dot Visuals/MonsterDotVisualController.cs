using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MonsterDotVisualController : ColorableDotVisualController, INumerableVisualController, IPreviewable
{
    private MonsterDot dot;
    private NumerableDotVisuals visuals;
    private readonly NumerableVisualControllerBase numerableVisualController = new();
    public override T GetGameObject<T>()
    {
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (MonsterDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<MonsterDotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
        numerableVisualController.Init(visuals);
        
    }


    public void UpdateNumbers(int amount)
    {
         numerableVisualController.UpdateNumbers(amount);
    }

    public IEnumerator PreviewHit(Type.HitType hitType)
    {
        float scaleDuration = 0.4f;
        UpdateNumbers(dot.TempNumber);

        visuals.digit1.transform.DOScale(Vector2.one * 1.3f, scaleDuration);
        visuals.digit2.transform.DOScale(Vector2.one * 1.3f, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);
        visuals.digit1.transform.DOScale(Vector2.one, scaleDuration);
        visuals.digit2.transform.DOScale(Vector2.one, scaleDuration);

    }

    public IEnumerator PreviewClear()
    {
        yield break;
    }
}
