using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public interface IPreviewable
{
    public IEnumerator PreviewHit(HitType hitType);
    
}
