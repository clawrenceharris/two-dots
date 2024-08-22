using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHitState : IState
{
   

    public virtual IEnumerator UpdateState(PreviewableStateManager context)
    {
        while(true){
            Debug.Log("Executing Preview Hit State");
            yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoHitPreviewAnimation();      
        }    
       
    }
}