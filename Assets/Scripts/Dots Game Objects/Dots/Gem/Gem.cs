using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening.Core;
public abstract class Gem : ConnectableDot, IExplodable, IPreviewable
{

    private static readonly List<IHittable> visited = new();

    public override IHitRule HitRule => new HitByConnectionRule();

    public override int HitsToClear => 1;

    public abstract IExplosionRule ExplosionRule {get;}

    public ExplosionType ExplosionType => ExplosionType.GemExplosion;

    public int HitsToExplode => 1;
   
    public new GemVisualController VisualController => GetVisualController<GemVisualController>();

    public IEnumerator Explode(List<IHittable> toHit, Board board, Action<IHittable> onComplete= null)
    { 
        HitCount++;
    
        yield return VisualController.Explode();

    }


    private void OnDestroy(){
        VisualController.Unsubscribe();
    }
    

    public bool ShouldPreviewClear(Board board)
    {
        return ShouldPreviewHit(board);
    }

    public bool ShouldPreviewHit(Board board)
    {
        if(board.FindDotsInRow<Gem>(Row)
            .Any((gem)=>
            gem.ExplosionRule.Validate(gem, board).Contains(this) && 
            gem.HitCount >= gem.HitsToExplode))
        {
            return true;
        }

        if(board.FindDotsInColumn<Gem>(Column)
            .Any((gem)=>
            gem.ExplosionRule.Validate(gem, board).Contains(this)&& 
            gem.HitCount >= gem.HitsToExplode))
        {
            return true;
        }
        if(HitRule.Validate(this, board) && DotTouchIO.IsInputActive){
            return true;
        }
        return false;
        
    }

    public override void Hit(HitType hitType)
    {
        //do nothing
    }

    public override void Deselect()
    {
        //do nothing
    }
}

