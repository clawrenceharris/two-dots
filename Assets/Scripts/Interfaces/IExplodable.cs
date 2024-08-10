using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IExplodable : IHittable, IBoardElement
{
    IExplosionRule ExplosionRule { get; }

    IEnumerator Explode(List<IHittable> toHit, Action<IHittable> onComplete);

    ExplosionType ExplosionType { get; }
    

    /// <summary>
    /// Number of hits required to explode the dot
    /// </summary>
    int HitsToExplode { get; }
}