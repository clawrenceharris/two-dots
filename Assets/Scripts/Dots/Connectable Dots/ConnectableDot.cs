
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public abstract class ConnectableDot : Dot, IConnectable,  IHittable
{
    public ConnectableDotVisualController VisualController => GetVisualController<ConnectableDotVisualController>();

    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new()
            {

                {
                    HitType.Square, new HitBySquareRule()
                }
            };
        }
    }

    public virtual void Disconnect()
    {
        HitType = HitType.None;
    }

    public virtual void Connect()
    {
        
        VisualController.AnimateSelectionEffect();

        
    }

    public virtual void Select()
    {
        VisualController.AnimateSelectionEffect();
    }

}
