using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHitState : IState
{
   

    public virtual IEnumerator UpdateState(PreviewableStateManager context)
    {
       
        
        while(true){
            if(context.Previewable.ShouldPreviewClear(context.Board)){
                context.ChangeState(new PreviewClearState());
            }
            yield return context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoHitPreviewAnimation();
            
           
        }
   
    }

}