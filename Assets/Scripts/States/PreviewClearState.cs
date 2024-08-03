using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PreviewClearState : IState
{

    public IEnumerator UpdateState(PreviewableStateManager context)
    {
        while(true){
            if(!context.Previewable.ShouldPreviewClear(context.Board)){
                context.ChangeState(new IdleState());
            }
            yield return CoroutineHandler.StartStaticCoroutine(context.DotsGameObject.GetVisualController<IPreviewableVisualController>().DoClearPreviewAnimation());
           
        }
        

        
    }

}