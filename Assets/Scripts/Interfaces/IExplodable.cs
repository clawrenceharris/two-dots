using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Type;
public interface IExplodable : IHittable
{
    Dictionary<HitType, IExplosionRule> ExplosionRules { get; }
    public IEnumerator Explode(List<IHittable> hittables, Action<IHittable> callback);
    public ExplosionType ExplosionType { get; }

}