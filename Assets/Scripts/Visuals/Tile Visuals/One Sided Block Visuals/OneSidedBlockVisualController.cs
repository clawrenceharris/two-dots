using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OneSidedBlockVisualController : HittableVisualController
{
    private OneSidedBlock tile;
    private HittableVisuals visuals;

    public override T GetGameObject<T>()
    {
        return tile as T ;
    }

    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        tile = (OneSidedBlock)dotsGameObject;
        visuals = dotsGameObject.GetComponent<HittableVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
    }

    protected override void SetColor()
    {
        spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.backgroundColor;
    }

    public override IEnumerator Hit(Type.HitType hitType)
    {
        yield return Clear();
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
            spriteRenderer.flipY = true;

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
