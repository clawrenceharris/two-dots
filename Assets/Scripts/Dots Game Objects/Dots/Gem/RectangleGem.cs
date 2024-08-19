using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleGem : Gem, IDirectional
{
    public override DotType DotType => DotType.RectangleGem;

    private readonly DirectionalBase directional = new();

    
    public int DirectionX { get => Mathf.Abs(directional.DirectionX); set => directional.DirectionX = value; }
    public int DirectionY { get =>  Mathf.Abs(directional.DirectionY); set => directional.DirectionY = value; }
    public new RectangleGemVisualController VisualController => GetVisualController<RectangleGemVisualController>();

    public override IExplosionRule ExplosionRule => new RectangleGemExplosionRule();

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        directional.Init(this);
    }
   public void ChangeDirection(int directionX, int directionY)
    {
       //do nothing
    }

    public Vector3 GetRotation()
    {
        Vector3 rotation = directional.GetRotation();
        return new Vector3(0,0, Mathf.Abs(rotation.z));
    }

    private void OnDestroy(){
        VisualController.Unsubscribe();
    }

    public override void InitDisplayController()
    {
        visualController = new RectangleGemVisualController();
        visualController.Init(this);
    }

    
}
