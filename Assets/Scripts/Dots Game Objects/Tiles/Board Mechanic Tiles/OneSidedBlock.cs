using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class OneSidedBlock : Tile, IDirectional, IHittable
{
    
    public override TileType TileType => TileType.OneSidedBlock;
    private int directionX;
    private int directionY;
    public HitType HitType { get; private set; }
    public int DirectionX { get => directionX; set => directionX = value; }
    public bool WasHit { get; set; }
    private readonly DirectionalBase directional = new();
    public int DirectionY { get => directionY; set => directionY = value; }
    public new OneSidedBlockVisualController VisualController => GetVisualController<OneSidedBlockVisualController>();
    public Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new() { { HitType.OneSidedBlock, new HitByNeighborRule()} };
        }
    }


    private int hitCount;
    public int HitCount { get => hitCount; set => hitCount = value; }

    public int HitsToClear => 1;

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        directional.Init(this);
    }


    public override void InitDisplayController()
    {
        visualController = new OneSidedBlockVisualController();
        visualController.Init(this);
    }

    public virtual IEnumerator Hit(HitType hitType, Action onHitChanged = null)
    {
        HitType = hitType;
        HitCount++;
        DotsObjectEvents.NotifyHit(this);
        if (hitType == HitType.BombExplosion)
        {
            yield return VisualController.DoBombHit();

        }
        onHitChanged?.Invoke();
        yield return VisualController.DoHitAnimation(hitType);

    }



    public IEnumerator Clear()
    {
        DotsObjectEvents.NotifyCleared(this);
        yield return VisualController.DoClearAnimation();
    }

    public void UndoHit()
    {
        HitType = HitType.None;
    }

    public void ChangeDirection(int directionX, int directionY)
    {
        directional.ChangeDirection(directionX, directionY);
    }

    public Vector3 GetRotation()
    {
        return directional.GetRotation();
    }
}
