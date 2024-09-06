using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterDotVisualController : ColorableDotVisualController, INumerableVisualController, IDirectionalVisualController
{
    private MonsterDot dot;
    private MonsterDotVisuals visuals;
    private SpriteManager spriteManager;
    private readonly DirectionalVisualController directionalVisualController = new();
    private readonly NumerableVisualController numerableVisualController = new();
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (MonsterDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<MonsterDotVisuals>();
        spriteManager = dotsGameObject.GetComponent<SpriteManager>();   
        directionalVisualController.Init(dot, visuals.DirectionalVisuals);
        numerableVisualController.Init(dot, visuals.NumerableVisuals);
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

    
    public IEnumerator DoRotateAnimation()
    {
        //do nothing; no rotation animation needed
        yield break;
    }

    public override void SetInitialColor()
    {   
        Color color = ColorSchemeManager.FromDotColor(dot.Color);
        visuals.spriteRenderer.color =  color;
        visuals.EyeLids.color = color;

    }


    public void UpdateRotation()
    {
        directionalVisualController.UpdateRotation();
    }

    public IEnumerator ScaleNumbers()
    {
       yield return numerableVisualController.ScaleNumbers();
    }
}
