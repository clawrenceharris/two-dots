using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHitState : IState
{
   

    public IEnumerator UpdateState(PreviewableStateManager context)
    {
        yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoHitPreviewAnimation();      
     
    }
}