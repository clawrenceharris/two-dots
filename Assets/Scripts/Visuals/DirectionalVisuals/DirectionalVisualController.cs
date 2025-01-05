using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DirectionalVisualController
{

    private VisualController visualController;
    private IDirectional Directional => visualController.GetGameObject<IDirectional>();
    public void Init(VisualController visualController)
    {
        this.visualController = visualController;
    }
    
}
