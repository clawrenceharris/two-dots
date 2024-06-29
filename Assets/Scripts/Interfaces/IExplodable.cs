using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;
public interface IExplodable : IHittable, IBoardElement
{
    Dictionary<HitType, IExplosionRule> ExplosionRules { get; }
    public IEnumerator Explode(List<IHittable> hittables, Action<IHittable> callback);
    public ExplosionType ExplosionType { get; }
    
   
    /// <summary>
    /// Number of hits required to explode the dot
    /// </summary>
    public int HitsToExplode { get; }
}