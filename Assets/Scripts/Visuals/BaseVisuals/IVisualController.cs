using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IVisualController
{
    public void Init(DotsGameObject dotsObject);
    public void SetColor(Color color);
    public void DisableSprites();
    public void EnableSprites();

    public T GetGameObject<T>() where T : class;
    public T GetVisuals<T>()where T : class;


}
