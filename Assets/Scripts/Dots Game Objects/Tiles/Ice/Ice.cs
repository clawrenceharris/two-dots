using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class Ice : Tile, IHittable
{
    public override TileType TileType => TileType.Ice;

    public new IceVisualController VisualController => GetVisualController<IceVisualController>();

    public HitType HitType { get; private set; }

    public bool WasHit { get; set; }
    public int HitCount { get; set; }

    public int HitsToClear => 3;

    public Dictionary<HitType, IHitRule> HitRules => new();

    public IEnumerator Clear()
    {
       yield return VisualController.DoClearAnimation();
    }

    public IEnumerator Hit(HitType hitType, System.Action onHitChanged = null)
    {
        throw new System.NotImplementedException();
    }

    public override void InitDisplayController()
    {
       visualController = new IceVisualController();
        visualController.Init(this);
    }
}
