using System.Collections;
using System.Collections.Generic;
using Animations;
using UnityEngine;

public class AnimatableVisualController : IAnimatableVisualController{
    private VisualController visualController;

    public void Init(VisualController visualController){
        this.visualController = visualController;
    }

   
    
    public IEnumerator Move(List<Vector3> positions) 
    {
       yield return visualController.Animate(new MoveAnimation{
            Target = positions
        });
    }

    public IEnumerator Rotate(Vector3 targetRotation)
    {   
        yield return visualController.Animate(new RotateAnimation{
            Target = targetRotation
        });

    }

 
   
    public IEnumerator Swap(Vector3 targetPosition)
    {
        yield return visualController.Animate(new SwapAnimation{
            Target = targetPosition
        });

    }

    
    public IEnumerator Drop(float targetY)
    {
        yield return visualController.Animate(new DropAnimation{
            Target = targetY
            
        });
    }

   
   
    
}