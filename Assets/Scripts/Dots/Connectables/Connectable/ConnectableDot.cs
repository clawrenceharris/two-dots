using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public abstract class ConnectableDot : Dot, IConnectable
{
    public override Dictionary<HitType, IHitRule> HitRules =>
        new ()
        {
            
            {
                HitType.Connection, new HitByConnectionRule()
            }
        };


    public new ConnectableDotVisualController VisualController => GetVisualController<ConnectableDotVisualController>();



    public abstract void Disconnect();

    public virtual void Connect(ConnectableDot dot)
    {

        VisualController.AnimateSelectionEffect();


    }

    public virtual void Select()
    {
       StartCoroutine(VisualController.AnimateSelectionEffect());
    }

    public abstract void Deselect();
    
}
