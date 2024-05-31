using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public interface IExplodable : IHittable
{
    Dictionary<HitType, IExplosionRule> ExplosionRules { get; }
    public IEnumerator Explode(List<IHittable> hittables);

}