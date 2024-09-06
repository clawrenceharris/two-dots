using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NestingDot : Dot, IPreviewable
{
    public override DotType DotType => DotType.NestingDot;

    public override DotsGameObjectData Replacement
    {
        get
        {
            return new(JSONLevelLoader.ToJsonDotType(DotType.Bomb))
            {
                col = Column,
                row = Row
            };

        }
    }
    public new NestingDotVisualController VisualController => GetVisualController<NestingDotVisualController>();

    public override int HitsToClear => 3;

    public override IHitRule HitRule => new HitByNeighborsRule();


    public override void Hit(HitType hitType)
    {
        HitCount++;
    }
    public override void InitDisplayController()
    {
        visualController = new NestingDotVisualController();
        visualController.Init(this);
    }

    public void OnConnectionChanged(LinkedList<ConnectableDot> connectedDots)
    {
        throw new NotImplementedException();
    }

    public bool ShouldPreview(PreviewableState state, Board board)
    {
        if(state == PreviewableState.PreviewingClear){
            return HitCount == HitsToClear -1;
        }
       
        return false;
        
    }

    public bool ShouldPreviewClear(Board board)
    {
        return  HitCount == HitsToClear -1;
;
    }

    public bool ShouldPreviewHit(Board board)
    {
        return HitRule.Validate(this, board);
    }
}
