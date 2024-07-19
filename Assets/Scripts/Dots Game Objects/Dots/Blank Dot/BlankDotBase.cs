using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlankDotBase : ConnectableDot, IConnectable, IPreviewable
{


    public override DotType DotType { get; }
    private new BlankDotBaseVisualController VisualController => GetVisualController<BlankDotBaseVisualController>();

    public bool IsPreviewing { get; protected set; }

    public virtual List<IHitRule> PreviewHitRules => new() { new HitByConnectionRule() };

    public override void Init(int column, int row)
    {
        base.Init(column, row);
    }
   
    
    /// <summary>
    /// /changes the color of the inner dot to the connection's color
    /// </summary>
    public void UpdateColor()
    {
        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        {
            VisualController.SetInnerColor(color);
        }
    }

    public override void Deselect()
    {
        StartCoroutine(VisualController.AnimateDeselectionEffect());
    }

   
    
    public virtual IEnumerator StartPreview(PreviewHitType hitType)
    {
        IsPreviewing = true;
        yield return VisualController.PreviewHit(hitType);

    }

    public virtual void StopPreview()
    {
        IsPreviewing = false;
    }
}

