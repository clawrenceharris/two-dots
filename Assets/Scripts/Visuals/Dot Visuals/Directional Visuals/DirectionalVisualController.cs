using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DirectionalVisualController : IDirectionalVisualController
{

    private IDirectionalVisuals visuals;
    private IDirectional directional;
    public void Init(IDirectional directional, IDirectionalVisuals visuals)
    {
        this.visuals = visuals;
        this.directional = directional;
    }
    public IEnumerator DoRotateAnimation()
    {
        
        Vector3 rotation = directional.GetRotation();
        DotsGameObject dotsGameObject = (DotsGameObject)directional;
        yield return dotsGameObject.transform.DOLocalRotate(rotation, visuals.RotationDuration)
                    .SetEase(visuals.RotationEase);

    }

    

}
