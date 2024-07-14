using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OneSidedBlockVisualController : TileVisualController, IHittableVisualController
{
    private OneSidedBlock tile;
    private HittableTileVisuals visuals;
    private readonly HittableVisualControllerBase hittableVisualController = new();

   
    public IEnumerator DoClearAnimation()
    {
        yield return hittableVisualController.DoClearAnimation();
    }

    public IEnumerator DoHitAnimation(Type.HitType hitType)
    {
        yield return hittableVisualController.DoHitAnimation(hitType);
    }

    public override T GetGameObject<T>() => tile as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (OneSidedBlock)dotsGameObject;
        visuals = dotsGameObject.GetComponent<HittableTileVisuals>();
        hittableVisualController.Init(tile, visuals);
        SetUp();
    }

    public override void SetInitialColor()
    {
        visuals.spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
    }

    protected override void SetUp()
    {
        tile.transform.localScale = Vector2.one * Board.offset;
        Rotate();
        base.SetUp();
    }

    private void Rotate()
    {
        Quaternion rotation = Quaternion.identity;

        if(tile.DirectionX < 0)
        {
            visuals.spriteRenderer.flipY = true;

            rotation = Quaternion.Euler(0, 0, 180);
        }
        else if(tile.DirectionY < 0)
        {
            rotation = Quaternion.Euler(0, 0, -90);
        }
        else if(tile.DirectionY >0)
        {
            rotation = Quaternion.Euler(0, 0, 90);

        }

        tile.transform.rotation = rotation;
    }

    

}
