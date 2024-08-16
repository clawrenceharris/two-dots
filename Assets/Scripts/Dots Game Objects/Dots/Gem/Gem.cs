using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class Gem : ConnectableDot, IExplodable, IPreviewable
{

    public override DotType DotType => DotType.Gem;
    private static readonly List<IHittable> visited = new();

    public override IHitRule HitRule => new HitByConnectionRule();

    public override int HitsToClear => 1;

    public IExplosionRule ExplosionRule => new GemExplosionRule();

    public ExplosionType ExplosionType => ExplosionType.GemExplosion;

    public int HitsToExplode => 1;


    public new GemVisualController VisualController => GetVisualController<GemVisualController>();
   

    public IEnumerator Explode(List<IHittable> toHit, Board board, Action<IHittable> onComplete= null)
    { 
        HitCount++;
       foreach(IHittable hittable in toHit){
            
            if(!visited.Contains(hittable) && hittable is Gem g){
                visited.Add(g);
                onComplete?.Invoke(g);                
            }
            
        }

        yield return new WaitForSeconds(0.5f);

    }

    
   
    public override void InitDisplayController()
    {
        visualController = new GemVisualController();
        visualController.Init(this);
    }

    

    public bool ShouldPreviewClear(Board board)
    {
        return ShouldPreviewHit(board);
    }

    public bool ShouldPreviewHit(Board board)
    {
        if(board.FindDotsInRow<Gem>(Row).Any((gem)=>gem.HitCount >= gem.HitsToExplode)){
            return true;
        }

        if(board.FindDotsInColumn<Gem>(Column).Any((gem)=>gem.HitCount >= gem.HitsToExplode)){
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

