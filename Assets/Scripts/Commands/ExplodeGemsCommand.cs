using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplodeGemsCommand : Command
{
    private int ongoingCoroutines = 0;

    List<IHittable> visitedGems = new();

    public override CommandType CommandType => CommandType.GemExplode;

    private void ExplodeGem<T>(T hittable, Board board)
    where T : IHittable
    {
        if(hittable is not Gem gem){
            return;
        }
        // Validate the explosion rule and get the list of elements to hit
        List<IHittable> toHit = gem.ExplosionRule.Validate(gem, board);
        
        CoroutineHandler.StartStaticCoroutine(gem.Hit(HitType.GemExplosion, null));

        ongoingCoroutines++;
            
        CoroutineHandler.StartStaticCoroutine(gem.Explode(toHit, board,(hittable)=>{
            HitCommand.DoHit(hittable, board, HitType.GemExplosion);
            ExplodeGem(gem, board);
        }),() => ongoingCoroutines--);
        

    }

       public override IEnumerator Execute(Board board)
    {
        
        List<Gem> gems = board.FindElementsOfType<Gem>();
        List<IHittable> hittables = new();
        foreach(Gem gem in gems){
            
            if(gem.HitRule.Validate(gem, board)){
                DidExecute = true;
                ExplodeGem(gem, board);
            }
                   
        }
                  
      
        yield return new WaitUntil(() => ongoingCoroutines == 0);
        
        yield return base.Execute(board);
    }
}
