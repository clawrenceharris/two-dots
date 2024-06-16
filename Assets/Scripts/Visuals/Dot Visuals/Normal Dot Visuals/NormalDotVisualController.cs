using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalDotVisualController : ColorDotVisualController
{
    public override IEnumerator Hit(Type.HitType hitType)
    {
        yield return base.Hit(hitType);
        Dot.transform.DOScale(Vector2.zero, Visuals.clearDuration);

    }
}
