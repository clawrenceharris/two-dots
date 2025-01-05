using System;
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


    private void OnDestroy(){
        VisualController.Unsubscribe();
    }

    public override void InitDisplayController()
    {
        visualController = new RectangleGemVisualController();
        visualController.Init(this);
    }

    public Vector2Int FindBestDirection(Board board, Func<DotsGameObject, bool> isValidTarget)
    {
        throw new NotImplementedException();
    }

    public Vector3 ToRotation(int dirX, int dirY)
    {
        Vector3 rotation = directional.ToRotation(dirX, dirY);
        return new Vector3(0,0, Mathf.Abs(rotation.z));
    }
}
