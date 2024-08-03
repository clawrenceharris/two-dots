using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IState
{
    IEnumerator UpdateState(PreviewableStateManager context);

}