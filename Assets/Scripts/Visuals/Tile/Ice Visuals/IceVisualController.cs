using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceVisualController : HittableVisualController
{
    private Tile tile;
    private IceVisuals visuals;

    public override T GetGameObject<T>() => tile as T;
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Ice)dotsGameObject;
        visuals = dotsGameObject.GetComponent<IceVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();

    }

    protected override void SetColor()
    {
        //do nothing
    }
}
