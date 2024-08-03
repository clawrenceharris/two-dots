using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleState : IState
{
    


    public IEnumerator UpdateState(PreviewableStateManager context)
    {
        
        
        while (true){
            if(context.Previewable.ShouldPreviewHit(context.Board)){
                context.ChangeState(new PreviewHitState());
            }
            yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoIdleAnimation();

        }   
    }

    
    
}

