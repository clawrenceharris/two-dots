using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHitState : IState
{
    public virtual void Enter(PreviewableStateManager context)
    {
        Debug.Log("Entering Previewing Hit State");
        
    }

    public virtual IEnumerator Execute(PreviewableStateManager context)
    {
        Debug.Log("Executing Previewing Hit State");
       
        
        yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoHitPreviewAnimation();
        
        

       
    }

    public virtual void Exit(PreviewableStateManager context)
    {
        Debug.Log("Exiting Previewing Hit State");
    }

    public virtual void OnConnectionChanged(PreviewableStateManager context)
    {
        
    }
}