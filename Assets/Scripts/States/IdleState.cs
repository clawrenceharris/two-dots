using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleState : IState
{
    
    public IEnumerator UpdateState(PreviewableStateManager context)
    {
        
        Debug.Log("Executing Idle State");
        yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoIdleAnimation();

        

    }

    
    
}

