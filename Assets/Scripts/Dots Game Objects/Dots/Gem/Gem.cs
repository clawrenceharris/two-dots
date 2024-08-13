using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Dot, IExplodable, IColorable, IPreviewable
{

    public override DotType DotType => DotType.Gem;

    public override IHitRule HitRule => new HitBySquareRule();

    public override int HitsToClear => 2;

    public IExplosionRule ExplosionRule => new GemExplosionRule();

    public ExplosionType ExplosionType => ExplosionType.GemExplosion;

    public int HitsToExplode => 1;

    public DotColor Color { get; set; }

    public new GemVisualController VisualController => GetVisualController<GemVisualController>();
   

    public IEnumerator Explode(List<IHittable> toHit, Action<IHittable> onComplete)
    { 
        yield return new WaitForSeconds(0.7f);
        yield return VisualController.Explode();
        foreach (IHittable hittable in toHit)
        {
            onComplete?.Invoke(hittable);
        }
        

    }

  
    public override void Hit(HitType hitType)
    {
       HitCount++;
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
        return HitRule.Validate(this, board);
    }

    public bool ShouldPreviewHit(Board board)
    {
        return HitRule.Validate(this, board);
    }
}

