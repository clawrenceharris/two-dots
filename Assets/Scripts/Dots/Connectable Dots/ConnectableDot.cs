
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public abstract class ConnectableDot : Dot, IConnectable
{


    private ConnectableDotVisualController VisualController
    {
        get
        {
            if (base.visualController is ConnectableDotVisualController connectableDotVisualController)
            {
                return connectableDotVisualController;
            }
            throw new InvalidCastException("Unable to cast base visualController to ClockDotVisualController");

        }
    }
    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new ()
            {

                {
                    HitType.Square, new HitBySquareRule()
                }

            };
        }
    }

   

    
    public override void InitDisplayController()
    {
        visualController = new ConnectableDotVisualController();
        base.visualController.Init(this);
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
