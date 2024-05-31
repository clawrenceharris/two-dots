using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using System;
public interface IHittable : IBoardElement
{
    public HitType HitType { get; }
    public IEnumerator Hit(HitType hitType);
    public void BombHit();
    public IEnumerator Clear();

    /// <summary>
    /// Number of hits made on the dot
    /// </summary>
    public int HitCount { get; }

    /// <summary>
    /// Number of hits required to clear the dot
    /// </summary>
    public int HitsToClear { get; }
    public Dictionary<HitType, IHitRule> HitRules { get; }
}
