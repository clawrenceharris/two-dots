using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPreviewable : IHittable
{
   
    bool ShouldPreviewClear(Board board);
    // void OnConnectionChanged(LinkedList<ConnectableDot> connectedDots);
    //  void UpdateState(PreviewableState state);
    bool ShouldPreviewHit(Board board);
}
