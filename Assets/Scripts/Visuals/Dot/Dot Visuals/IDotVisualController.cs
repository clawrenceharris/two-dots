using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDotVisualController : IVisualController
   
{
    public IEnumerator Pulse();
 
}