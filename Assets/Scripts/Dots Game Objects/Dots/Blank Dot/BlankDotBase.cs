using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public abstract class BlankDotBase : ConnectableDot, IBlank, IConnectable, IColorable, IPreviewable
{


    public override DotType DotType { get; }
    private new BlankDotBaseVisualController VisualController => GetVisualController<BlankDotBaseVisualController>();

    public DotColor Color { get; set; }
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

   

    public override void Connect(ConnectableDot dot)
    {
        base.Connect(dot); 
        if (dot is IColorable colorable)
            Color = colorable.Color;
    }
    
    public virtual IEnumerator StartPreview(PreviewHitType hitType)
    {
        IsPreviewing = true;
        StartCoroutine(VisualController.PreviewHit(hitType));
        yield return null;

    }

    public virtual void StopPreview()
    {
        IsPreviewing = false;
    }
}

