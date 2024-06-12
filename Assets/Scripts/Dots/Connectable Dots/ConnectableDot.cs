
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
            if (visualController is ConnectableDotVisualController connectableDotVisualController)
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
                },
                {
                    HitType.Explosion, new HitByExplosionRule()
                }

            };
        }
    }

   

    
    public override void InitDisplayController()
    {
        visualController = new ConnectableDotVisualController();
        visualController.Init(this);
    }


    public virtual void Disconnect()
    {

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
