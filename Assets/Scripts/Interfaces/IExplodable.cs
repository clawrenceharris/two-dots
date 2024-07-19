using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IExplodable : IHittable, IBoardElement
{
    Dictionary<HitType, IExplosionRule> ExplosionRules { get; }
    //IEnumerator Explode(Dictionary<IExplodable, List<IHittable>> explodables, Action<IHittable> onComplete);
    IEnumerator Explode(List<IHittable> toHit, Action<IHittable> onComplete);

    ExplosionType ExplosionType { get; }
    

    /// <summary>
    /// Number of hits required to explode the dot
    /// </summary>
    int HitsToExplode { get; }
}