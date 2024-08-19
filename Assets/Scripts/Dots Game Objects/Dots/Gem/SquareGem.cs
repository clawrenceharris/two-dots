using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGem : Gem
{
    public override DotType DotType => DotType.SquareGem;

    public new SquareGemVisualController VisualController => GetVisualController<SquareGemVisualController>();

    public override IExplosionRule ExplosionRule => new SquareGemExplosionRule();

    private void OnDestroy(){
        VisualController.Unsubscribe();
    }

     public override void InitDisplayController()
    {
        visualController = new SquareGemVisualController();
        visualController.Init(this);
    }

}
