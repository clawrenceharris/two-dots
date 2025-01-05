using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceVisualController : TileVisualController, IHittableVisualController
{
    private Ice tile;
    private IceVisuals visuals;
   
    public override T GetGameObject<T>() => tile as T;
    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (Ice)dotsGameObject;
        visuals = dotsGameObject.GetComponent<IceVisuals>();
        base.Init(dotsGameObject);
    }

    protected override void SetUp()
    {
        base.SetUp();
        UpdateSprite();
        tile.transform.localScale = Vector2.one * (Board.offset - Board.offset /5);

    }

    public override void SetInitialColor()
    {
        //do nothing
    }


    public void UpdateSprite()
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

    public IEnumerator Hit()
    {
        UpdateSprite();
        yield return null;
    }

    public IEnumerator Clear(float duration)
    {
        yield return null;
    }
}
