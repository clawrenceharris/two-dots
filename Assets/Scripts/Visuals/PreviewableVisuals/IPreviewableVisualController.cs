using System.Collections;
using UnityEngine;

public interface IPreviewableVisualController{
    
    /// <summary>
    /// Starts the object's clear preview animation.
    /// </summary>
    public IEnumerator PreviewClear();
  

    /// <summary>
    /// Starts the object's hit preview animation.
    /// </summary>
    public IEnumerator PreviewHit();
   
    /// <summary>
    /// Starts the object's idle animation.
    /// </summary>
    public IEnumerator Idle();
   


}