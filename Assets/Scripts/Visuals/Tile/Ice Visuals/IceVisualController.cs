using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public class IceVisualController : TileVisualController, IHittableVisualController
{
    private Ice tile;
    private IceVisuals visuals;
   
    private readonly HittableVisualControllerBase hittableVisualController = new();
    public override T GetGameObject<T>() => tile as T;
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Ice)dotsGameObject;
        visuals = dotsGameObject.GetComponent<IceVisuals>();
        hittableVisualController.Init(tile, visuals);
        SetUp();
    }

    protected override void SetUp()
    {
        base.SetUp();
        tile.transform.localScale = Vector2.one * (Board.offset - Board.offset /4);

        SetSprite();
    }

    protected override void SetColor()
    {
        //do nothing
    }

    public IEnumerator DoBombHit()
    {
        yield return hittableVisualController.DoBombHit();
    }


    private void SetSprite()
    {
        if (tile.HitCount == 1)
        {
            visuals.cracks.sprite = visuals.cracksHit1;
        }

        if (tile.HitCount == 2)
        {
            visuals.cracks.sprite = visuals.cracksHit2;
        }
    }

    public IEnumerator DoHitAnimation(HitType hitType)
    {
        SetSprite();
        yield return null;
    }

    public IEnumerator DoClearAnimation()
    {
        yield return hittableVisualController.DoClearAnimation();
    }
}
