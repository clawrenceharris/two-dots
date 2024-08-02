using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleState : IState
{
    public void Enter(PreviewableStateManager context)
    {
        
    }

    public IEnumerator Execute(PreviewableStateManager context)
    {
        yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoIdleAnimation();
    }

    public void Exit(PreviewableStateManager context)
    {
    }

    public void OnConnectionChanged(PreviewableStateManager context)
    {
       
    }
}

