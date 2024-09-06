using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public interface IHittable
{
    HitType HitType { get; }
    IEnumerator Hit(HitType hitType, Action onHitComplete = null);

    bool WasHit { get; set; }
   
    /// <summary>
    /// Number of hits made on the object
    /// </summary>
    int HitCount { get; set; }
    /// <summary>
    /// Number of hits required to clear the object
    /// </summary>
    int HitsToClear { get; }
    IHitRule HitRule { get; }
    IEnumerator Clear();


}
