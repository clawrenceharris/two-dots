using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static Type;


public class BombDot : Dot, IExplodable
{
    

    public override DotType DotType => DotType.Bomb;

    public Dictionary<HitType, IExplosionRule> ExplosionRules => new() { { HitType.BombExplosion, new BombExplosionRule() } };
    public override int HitsToClear => 1;

    public override Dictionary<HitType, IHitRule> HitRules => new() { { HitType.BombExplosion, new BombExplosionHitRule() } };

    public static List<IHittable> Hits { get; } = new();// the list of hittables all Bomb objects have hit

    public ExplosionType ExplosionType => ExplosionType.BombExplosion;

    public static event Action<IHittable> onBombExploded;
    public new BombDotVisualController VisualController => GetVisualController<BombDotVisualController>();
    public override void InitDisplayController()
    {
        visualController = new BombDotVisualController();
        visualController.Init(this);
    }


    

    private void OnDisable()
    {
        Hits.Clear();
    }


    public IEnumerator Explode(List<IHittable> hittables, Action<IHittable> callback)
    {
        List<IHittable> hits = new(); // the list of hittables this current Bomb object has hit
        

        foreach (IHittable hittable in hittables)
        {
            if (hittable is BombDot)
            {
                continue;
            }

            StartCoroutine(VisualController.AnimateLine(hittable));
            Hits.Add(hittable);
            hits.Add(hittable);
            yield return new WaitForSeconds(0.05f); // wait before animating next line

        }


        foreach (IHittable hittable in hits)
        {
            yield return new WaitForSeconds(0.01f); // wait before invoking completion
            callback?.Invoke(hittable);
        }

    }

    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        return base.Hit(hitType);
    }


}
   

