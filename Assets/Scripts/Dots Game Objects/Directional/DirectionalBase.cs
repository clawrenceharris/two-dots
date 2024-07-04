using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalBase : IDirectional
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }
    private IDirectional directional;
    public DotsGameObject GetGameObject() => (DotsGameObject)directional;
    public T GetGameObject<T>() where T : DotsGameObject => (T)directional;

    public  IDirectionalVisualController VisualController
    {
        get
        {
            DotsGameObject dotsObject = GetGameObject();
            return dotsObject.GetVisualController<IDirectionalVisualController>();
        }
    }

    public Vector3 GetRotation()
    {
        Vector3 rotation = Vector3.zero;
        if (DirectionY < 0)
        {
            rotation = new Vector3(0, 0, 180);
        }

        if (DirectionX < 0)
        {
            rotation = new Vector3(0, 0, 90);

        }
        if (DirectionX > 0)
        {
            rotation = new Vector3(0, 0, -90);

        }
        return rotation;
    }

    public void Init(IDirectional directional)
    {
        this.directional = directional;
    }

    public void ChangeDirection(int directionX, int directionY)
    {
        DirectionX = directionX;
        DirectionY = directionY;

        CoroutineHandler.StartStaticCoroutine(VisualController.DoRotateAnimation());
    }
}
