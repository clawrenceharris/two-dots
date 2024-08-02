using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PreviewClearState : IState
{
    public void Enter(PreviewableStateManager context)
    {
        
        Debug.Log("Entering Previewing Clearing State");
    }

    public IEnumerator Execute(PreviewableStateManager context)
    {
        Debug.Log("Executing Clearing State");
        yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoClearPreviewAnimation();
        


        
    }

    public void Exit(PreviewableStateManager context)
    {
        Debug.Log("Exiting Previewing Clearing State");

    }

    public void OnConnectionChanged(PreviewableStateManager context)
    {
        
    }
}