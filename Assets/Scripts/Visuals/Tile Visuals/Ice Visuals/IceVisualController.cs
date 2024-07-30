using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceVisualController : TileVisualController, IHittableVisualController
{
    private Ice tile;
    private IceVisuals visuals;
   
    private readonly HittableVisualController hittableVisualController = new();
    public override T GetGameObject<T>() => tile as T;
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Ice)dotsGameObject;
        visuals = dotsGameObject.GetComponent<IceVisuals>();
        hittableVisualController.Init(tile, visuals);
        base.Init(dotsGameObject);
    }

    protected override void SetUp()
    {
        base.SetUp();
        tile.transform.localScale = Vector2.one * (Board.offset - Board.offset /5);

        SetSprite();
    }

    public override void SetInitialColor()
    {
        //do nothing
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

    public IEnumerator Hit(HitType hitType)
    {
        SetSprite();
        if(tile.HitCount >= tile.HitsToClear)
        {
            yield return Clear();
        }
    }

    public IEnumerator Clear()
    {
        yield return hittableVisualController.Clear();
    }
}
