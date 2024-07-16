using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public interface IPreviewable : IHittable
{
    bool IsPreviewing { get;}
    List<IHitRule> PreviewHitRules { get; }

    IEnumerator StartPreview(PreviewHitType hitType);
    void StopPreview();
}
