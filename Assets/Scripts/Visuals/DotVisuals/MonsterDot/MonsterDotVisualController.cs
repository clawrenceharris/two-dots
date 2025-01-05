using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterDotVisualController : ColorableDotVisualController, 
INumerableVisualController,
IPreviewableVisualController
{
    private MonsterDot dot;
    private MonsterDotVisuals visuals;
    private SpriteManager spriteManager;
    private readonly DirectionalVisualController directionalVisualController = new();
    private readonly NumerableVisualController numerableVisualController = new();
    private readonly PreviewableVisualController previewableVisualController = new();
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (MonsterDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<MonsterDotVisuals>();
        spriteManager = dotsGameObject.GetComponent<SpriteManager>();   
        directionalVisualController.Init(this);
        numerableVisualController.Init(dot, visuals.NumerableVisuals);
        previewableVisualController.Init(this);
        base.Init(dotsGameObject);
    }

    

    public void UpdateNumbers(int amount)
    {
         numerableVisualController.UpdateNumbers(amount);
    }


    public IEnumerator DoMove(int col, int row)
    {
        
        spriteManager.BringSpritesToTop();
        yield return dot.transform.DOMove(new Vector2(col, row) * Board.offset, MonsterDotVisuals.MoveDuration);

        yield return new WaitForSeconds(0.8f);
        spriteManager.BringSpritesBack();
    }


    public override void SetInitialColor()
    {   
        Color color = ColorSchemeManager.FromDotColor(dot.Color);
        visuals.spriteRenderer.color =  color;
        visuals.EyeLids.color = color;

    }


    public IEnumerator ScaleNumbers()
    {
       yield return numerableVisualController.ScaleNumbers();
    }

    public IEnumerator PreviewClear()
    {
        yield return previewableVisualController.PreviewClear();
    }

    public IEnumerator PreviewHit()
    {
        yield return previewableVisualController.PreviewHit();
    }

    public IEnumerator Idle()
    {
        yield return previewableVisualController.Idle();
    }
}
