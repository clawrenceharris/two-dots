using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleGem : Gem
{
    private void OnDestroy(){
        VisualController.Unsubscribe();
    }
}
