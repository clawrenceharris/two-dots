
using System.Collections;
using System.Collections.Generic;
using static Type;
using Color = UnityEngine.Color;

public class BlankDot : BlankDotBase
{
    public override DotType DotType => DotType.BlankDot;

    public override int HitsToClear => 1;

    

    public override void Init(int column, int row)
    {
        base.Init(column, row);
    }

    

    public override void InitDisplayController()
    {
        visualController = new BlankDotVisualController();
        visualController.Init(this);
    }
    
    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        return base.Hit(hitType);
    }


}
