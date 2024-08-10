using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplodeGemsCommand : Command
{
    public override CommandType CommandType => CommandType.GemExplode;

    public override IEnumerator Execute(Board board)
    {
        int ongoingCoroutines = 0;
        List<Gem> gems = board.FindElementsOfType<Gem>();
        if(gems.Count == 0)
        {
            yield break;
        }
        List<Gem> gemsToExplode = new();


        foreach(Gem gem in gems){
            if(gem.HitRule.Validate(gem, board)){
                ongoingCoroutines++;
                
                gemsToExplode.Add(gem);
            }
        }
        yield return new ExplodeCommand(gemsToExplode.OfType<IExplodable>().ToList(), CommandType.GemExplode).Execute(board);
        yield return base.Execute(board);
    }
}
