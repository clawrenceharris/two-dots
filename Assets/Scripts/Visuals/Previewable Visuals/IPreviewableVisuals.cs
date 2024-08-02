using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPreviewableVisualController
{
    IEnumerator DoIdleAnimation();

    IEnumerator DoHitPreviewAnimation();

    IEnumerator DoClearPreviewAnimation();
}
