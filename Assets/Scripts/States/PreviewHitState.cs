using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHitState : IState
{
   

    public IEnumerator UpdateState(PreviewableStateManager context)
    {
        yield return context.DotsGameObject.VisualController.Animate(new PreviewHitAnimation(), AnimationLayer.PreviewLayer);      
     
    }
}