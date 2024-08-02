using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IState
{
    void Enter(PreviewableStateManager context);

    IEnumerator Execute(PreviewableStateManager context);
    void Exit(PreviewableStateManager context);

    void OnConnectionChanged(PreviewableStateManager context);
}