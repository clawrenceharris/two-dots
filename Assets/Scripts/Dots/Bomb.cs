using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static Type;

public class Bomb : Dot, IExplodable
{


    public override DotType DotType => DotType.Bomb;

    public Dictionary<HitType, IExplosionRule> ExplosionRules => new() { { HitType.BombExplosion, new BombExplosionRule() } };
    public override int HitsToClear => 0;

    public override Dictionary<HitType, IHitRule> HitRules => new();

    public static List<IHittable> Hits { get; } = new();

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
        Hits.Clear();
    }


    public IEnumerator Explode(List<IHittable> hittables, Action<IHittable> callback)
    {
        List<Coroutine> lineCoroutines = new(); // Store coroutines for each line

        foreach (IHittable hittable in hittables)
        {
            if (hittable is Bomb)
            {
                continue;
            }
            
            Coroutine lineCoroutine = StartCoroutine(VisualController.AnimateLine(hittable, () =>
            {
                callback?.Invoke(hittable);
            }));
            lineCoroutines.Add(lineCoroutine);
            Hits.Add(hittable);


        }

        // Wait for all line animations to finish
        foreach (Coroutine coroutine in lineCoroutines)
        {
            yield return coroutine;
        }

    }




}
   

