using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatableVisualController{
    
    /// <summary>
    /// Starts the object's movement animation to the specified target positions using the predefined animation settings.
    /// </summary>
    IEnumerator Move(List<Vector3> positions);
    
   
    
    /// <summary>
    /// Starts the objects rotation animation to the specified target rotation using the predefined animation settings.
    /// </summary>
    IEnumerator Rotate(Vector3 targetRotation);
    

    /// <summary>
    /// Starts the object's dropping animation to the specified Y position.
    /// </summary>
    IEnumerator Drop(float targetY);

    /// <summary>
    /// Starts the object's swapping animation to the specified target position.
    /// </summary>
    IEnumerator Swap(Vector3 targetPosition);
}