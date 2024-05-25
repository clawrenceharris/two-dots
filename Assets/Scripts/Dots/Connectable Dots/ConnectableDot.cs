
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public abstract class ConnectableDot : Dot, IConnectable
{


    public override Dictionary<HitType, IHitRule> HitRules
 {
        get
        {
            return new ()
            {


                {
                    HitType.Connection, new HitByConnectionRule()
                },
                {
                    HitType.Square, new HitBySquareRule()
                }

            };
        }
    }
    public abstract void Connect();

    public override void InitDisplayController()
    {
        visualController = new ConnectableDotVisualController();
        visualController.Init(this);
    }


    public abstract void Disconnect(); 

    public abstract void Select();
    
}
