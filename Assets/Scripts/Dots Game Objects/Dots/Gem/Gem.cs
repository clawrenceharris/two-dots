using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Dot, IExplodable, IColorable, IPreviewable
{

    public override DotType DotType => DotType.Gem;
    private static readonly List<IHittable> visited = new();

    public override IHitRule HitRule => new HitBySquareRule();

    public override int HitsToClear => 1;

    public IExplosionRule ExplosionRule => new GemExplosionRule();

    public ExplosionType ExplosionType => ExplosionType.GemExplosion;

    public int HitsToExplode => 1;

    public DotColor Color { get; set; }

    public new GemVisualController VisualController => GetVisualController<GemVisualController>();
   

    public IEnumerator Explode(List<IHittable> toHit, Board board, Action<IHittable> onComplete= null)
    { 
        HitCount++;
        foreach(IHittable hittable in toHit){
            
            if(!visited.Contains(hittable)){
                yield return new WaitForSeconds(hittable is Gem ? 0.2f : 0f);
                onComplete?.Invoke(hittable);
                visited.Add(hittable);

            }
            
        }
        CoroutineHandler.StartStaticCoroutine(VisualController.Explode());

    }

    private void OnDestroy(){
        VisualController.Unsubscribe();
    }
   
    public override void InitDisplayController()
    {
        visualController = new GemVisualController();
        visualController.Init(this);
    }

    public void Select()
    {
        StartCoroutine(VisualController.AnimateSelectionEffect());
    }

    public bool ShouldPreviewClear(Board board)
    {
        return ShouldPreviewHit(board);
    }

    public bool ShouldPreviewHit(Board board)
    {
        return HitRule.Validate(this, board) && DotTouchIO.IsInputActive;
    }

    public override void Hit(HitType hitType)
    {
        
    }
}

