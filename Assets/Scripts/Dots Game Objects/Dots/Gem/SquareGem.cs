using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGem : Gem
{
    private void OnDestroy(){
        VisualController.Unsubscribe();
    }
}
