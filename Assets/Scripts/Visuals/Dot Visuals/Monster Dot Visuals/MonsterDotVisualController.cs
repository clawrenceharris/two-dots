using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterDotVisualController : ColorableDotVisualController, INumerableVisualController, IDirectionalVisualController, IPreviewableVisualController
{
    private MonsterDot dot;
    private MonsterDotVisuals visuals;
    private readonly DirectionalVisualController directionalVisualController = new();
    private readonly NumerableVisualController numerableVisualController = new();
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (MonsterDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<MonsterDotVisuals>();
        directionalVisualController.Init(dot, visuals.directionalVisuals);
        numerableVisualController.Init(dot, visuals.numerableVisuals);
        base.Init(dotsGameObject);

    }

    

    public void UpdateNumbers(int amount)
    {
         numerableVisualController.UpdateNumbers(amount);
    }

    

    public IEnumerator ScaleNumbers()
    {
        float scaleDuration = 0.2f;

        visuals.numerableVisuals.Digit1.transform.DOScale(Vector2.one * 1.8f, scaleDuration);
        visuals.numerableVisuals.Digit2.transform.DOScale(Vector2.one * 1.8f, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);
        visuals.numerableVisuals.Digit1.transform.DOScale(Vector2.one, scaleDuration);
        visuals.numerableVisuals.Digit2.transform.DOScale(Vector2.one, scaleDuration);
    }

   
    public IEnumerator DoMove(int col, int row)
    {
        for(int i = 0; i < visuals.sprites.Length; i++)
        {
            visuals.sprites[i].sortingOrder += 100;

        }
        yield return dot.transform.DOMove(new Vector2(col, row) * Board.offset, MonsterDotVisuals.moveDuration);

        yield return new WaitForSeconds(0.8f);
        
    }

    public override IEnumerator Hit(HitType hitType)
    {
        yield return ScaleNumbers();

    }

    public IEnumerator DoRotateAnimation()
    {
        yield break;
    }

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = ColorSchemeManager.FromDotColor(dot.Color); ;
    }

    public IEnumerator DoClearPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoHitPreviewAnimation()
    {
        int connectionCount = ConnectionManager.ToHit.Count;

        dot.TempNumber = Mathf.Clamp(dot.CurrentNumber - connectionCount, 0, int.MaxValue);

        UpdateNumbers(dot.TempNumber);
        yield return ScaleNumbers();
    }

    public IEnumerator DoIdleAnimation()
    {
        throw new System.NotImplementedException();
    }
}
