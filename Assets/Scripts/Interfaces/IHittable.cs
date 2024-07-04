using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using System;
public interface IHittable : IBoardElement
{
    HitType HitType { get; }
    IEnumerator Hit(HitType hitType, Action onHitChanged = null);

    bool WasHit { get; set; }
   
    /// <summary>
    /// Number of hits made on the dot
    /// </summary>
    int HitCount { get; set; }
    /// <summary>
    /// Number of hits required to clear the dot
    /// </summary>
    int HitsToClear { get; }
    Dictionary<HitType, IHitRule> HitRules { get; }
    IEnumerator Clear();

}
