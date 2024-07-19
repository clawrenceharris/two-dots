using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPreviewable : IHittable
{
    bool IsPreviewing { get;}
    List<IHitRule> PreviewHitRules { get; }

    IEnumerator StartPreview(PreviewHitType hitType);
    void StopPreview();
}
