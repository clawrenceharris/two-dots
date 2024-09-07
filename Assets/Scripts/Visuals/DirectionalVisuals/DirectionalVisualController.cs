using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DirectionalVisualController : IDirectionalVisualController
{

    private VisualController visualController;
    private IDirectional Directional => visualController.GetGameObject<IDirectional>();
    public void Init(VisualController visualController)
    {
        this.visualController = visualController;
    }
    

    public void SetRotation()
    {
        DotsGameObject dotsGameObject = (DotsGameObject)Directional;
        Vector3 rotation = Directional.GetRotation();
        dotsGameObject.transform.localRotation = Quaternion.Euler(rotation);
    }
}
