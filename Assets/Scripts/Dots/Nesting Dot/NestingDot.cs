using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class NestingDot : Dot, IPreviewable
{
    public override DotType DotType => DotType.NestingDot;
    public override CommandType CommandType => CommandType.NestingDot;

    public override DotsGameObjectData ReplacementDot
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

    public override Dictionary<HitType, IHitRule> HitRules =>
        new(){{HitType.NestingDot, new HitByNeighborsRule()}};

    public override IEnumerator Execute(Board board)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        yield return base.Hit(hitType);
    }
    public override void InitDisplayController()
    {
        visualController = new NestingDotVisualController();
        visualController.Init(this);
    }

    public IEnumerator PreviewClear()
    {
        yield return VisualController.PreviewClear();
    }

    public IEnumerator PreviewHit(HitType hitType)
    {
        yield break;
    }
}
