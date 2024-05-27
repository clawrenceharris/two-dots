using System.Collections;

using static Type;
public class HitTilesCommand : Command
{
    public override CommandType CommandType => CommandType.HitTiles;

    public override IEnumerator Execute(Board board)
    {
        foreach(Tile tile in board.Tiles)
        {
            if(tile is not IHittable hittable)
            {
                continue;
            }
            foreach (HitType hitType in hittable.HitRules.Keys)
            {
                hittable.HitRules.TryGetValue(hitType, out IHitRule rule);

                if (rule.Validate(hittable, board ))
                {

                    hittable.Hit(hitType);
                }
            }
            
        }
        
        return base.Execute(board);
    }

}
