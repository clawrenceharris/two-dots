using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Dot, IExplodable, IColorable, IDirectional
{
    public override DotType DotType => DotType.Gem;

    public override IHitRule HitRule => new HitBySquareRule();

    public override int HitsToClear => 1;

    public IExplosionRule ExplosionRule => new GemExplosionRule();

    public ExplosionType ExplosionType => ExplosionType.GemExplosion;

    public int HitsToExplode => 1;

    public DotColor Color { get; set; }
   private readonly DirectionalBase directional = new();

    public int DirectionX { get => directional.DirectionX; set => directional.DirectionX = value; }
    public int DirectionY { get => directional.DirectionY; set => directional.DirectionY = value; }


    public override void Init(int column, int row)
    {
        base.Init(column, row);
        directional.Init(this);
        
    }
    public void ChangeDirection(int directionX, int directionY)
    {
        
    }

    public IEnumerator Explode(List<IHittable> toHit, Action<IHittable> onComplete)
    {
        yield break;
    }

    public Vector3 GetRotation()
    {
        return Vector3.one;
    }

    public override void Hit(HitType hitType)
    {
       
    }

    public override void InitDisplayController()
    {
        visualController = new GemVisualController();
        visualController.Init(this);
    }
}

