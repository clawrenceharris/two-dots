using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PreviewClearState : IState
{

    public IEnumerator UpdateState(PreviewableStateManager context)
    {
        while(true){
            Debug.Log("Executing Preview Clear State");
            yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoClearPreviewAnimation();
       
        }
       
        
            
        
    }

}