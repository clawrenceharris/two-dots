using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ConnectableDot : Dot, IConnectable
{
    public override IHitRule HitRule => new HitByConnectionRule();
            
        


    public new ColorableDotVisualController VisualController => GetVisualController<ColorableDotVisualController>();

    public virtual DotColor Color { get; set; }

    public virtual void Disconnect()
    {
        HitType = HitType.None;
        Deselect();

    }

    public virtual void Connect(ConnectableDot dot)
    {

        StartCoroutine(VisualController.AnimateSelectionEffect());


    }

    public virtual void Select()
    {
        StartCoroutine(VisualController.AnimateSelectionEffect());
    }

    public abstract void Deselect();
    
}
