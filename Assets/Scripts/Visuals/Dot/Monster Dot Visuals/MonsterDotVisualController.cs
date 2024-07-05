using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MonsterDotVisualController : ConnectableDotVisualController, INumerableVisualController, IPreviewable, IDirectionalVisualController
{
    private MonsterDot dot;
    private MonsterDotVisuals visuals;
    private readonly DirectionalVisualController directionalVisualController = new();
    private readonly NumerableVisualControllerBase numerableVisualController = new();
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

    public IEnumerator PreviewHit(Type.HitType hitType)
    {
        float scaleDuration = 0.4f;
        UpdateNumbers(dot.TempNumber);

        visuals.numerableVisuals.Digit1.transform.DOScale(Vector2.one * 1.3f, scaleDuration);
        visuals.numerableVisuals.Digit2.transform.DOScale(Vector2.one * 1.3f, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);
        visuals.numerableVisuals.Digit1.transform.DOScale(Vector2.one, scaleDuration);
        visuals.numerableVisuals.Digit2.transform.DOScale(Vector2.one, scaleDuration);

    }

    public IEnumerator PreviewClear()
    {
        yield break;
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


    public IEnumerator DoRotateAnimation()
    {
        yield break;
    }

    protected override void SetColor()
    {
        visuals.spriteRenderer.color = ColorSchemeManager.FromDotColor(dot.Color); ;
    }
}
