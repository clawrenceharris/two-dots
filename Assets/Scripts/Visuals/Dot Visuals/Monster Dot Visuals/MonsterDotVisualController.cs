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
        ConnectionManager.onDotConnected += OnConnectionChanged;
        ConnectionManager.onDotDisconnected += OnConnectionChanged;
    }

    

    public void UpdateNumbers(int amount)
    {
         numerableVisualController.UpdateNumbers(amount);
    }

    
    private void OnConnectionChanged(ConnectableDot _){
       
        numerableVisualController.UpdateNumberByConnectionCount(dot);
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
        yield break;
    }

    public IEnumerator DoIdleAnimation()
    {
        //TODO: Blinking animation
        yield break;
    }
}
