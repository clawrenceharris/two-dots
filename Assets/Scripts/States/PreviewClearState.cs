using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PreviewClearState : IState
{

    public IEnumerator UpdateState(PreviewableStateManager context)
    {
        yield return context.DotsGameObject.VisualController.Animate(new PreviewClearAnimation(), AnimationLayer.PreviewLayer);      
    }

}