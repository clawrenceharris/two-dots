using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static Type;


public class HitByBombExplosion : IHitRule
{
    public bool Validate(IHittable hittable, Board board)
    {
        return true;
    }
}

public class Bomb : Dot, IExplodable
{
    

    public override DotType DotType => DotType.Bomb;

    public Dictionary<HitType, IExplosionRule> ExplosionRules => new() { { HitType.BombExplosion, new BombExplosionRule() } };
    public override int HitsToClear => 1;

    public override Dictionary<HitType, IHitRule> HitRules => new() { { HitType.BombExplosion, new HitByBombExplosion() } };

    public static List<IHittable> AllHits { get; } = new();

    public ExplosionType ExplosionType => ExplosionType.BombExplosion;

    public static event Action<IHittable> onBombExploded;
    public BombDotVisualController VisualController => GetVisualController<BombDotVisualController>();
    public override void InitDisplayController()
    {
        visualController = new BombDotVisualController();
        visualController.Init(this);
    }


    

    private void OnDisable()
    {
        AllHits.Clear();
    }


    public IEnumerator Explode(List<IHittable> hittables, Action<IHittable> callback)
    {
        List<Coroutine> lineCoroutines = new();
        List<IHittable> hits = new();
        hittables.Sort((a, b) =>
        {
            int result = a.Column.CompareTo(b.Column);
            if (result == 0)
            {
                result = a.Row.CompareTo(b.Row);
            }
            return result;
        });

        foreach (IHittable hittable in hittables)
        {
            if (hittable is Bomb)
            {
                continue;
            }

            StartCoroutine(VisualController.AnimateLine(hittable));
            AllHits.Add(hittable);
            hits.Add(hittable);
            yield return new WaitForSeconds(0.01f);

        }


        foreach (IHittable hittable in hits)
        {
            yield return new WaitForSeconds(0.05f);
            callback?.Invoke(hittable);
        }

    }

    public override IEnumerator Hit(HitType hitType)
    {
        hitCount++;
        return base.Hit(hitType);
    }


}
   

