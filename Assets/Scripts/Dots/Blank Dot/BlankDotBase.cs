using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public abstract class BlankDotBase : ConnectableDot, IBlank, IConnectable, IColorable
{


    public override DotType DotType { get; }
    private new BlankDotVisualController VisualController => GetVisualController<BlankDotVisualController>();

    public DotColor Color { get; set; }

    public override void Init(int column, int row)
    {
        base.Init(column, row);
        SubscribeToEvents();
    }
   
    private void SubscribeToEvents()
    {
        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
    }

    private void UnsubscribeToEvents()
    {
        ConnectionManager.onDotConnected -= OnDotConnected;
        ConnectionManager.onDotDisconnected -= OnDotDisconnected;
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    
    protected virtual void OnDotDisconnected(ConnectableDot dot)
    {

        //if any of the blank dots are not set to be hit by the connection then visually disconnect it
        if (!ConnectionManager.ToHit.Contains(this))
        {
            Disconnect();
        }
        //update the inner dot's color to match the connection color
        UpdateColor();
    }
    protected virtual void OnConnectionEnded(LinkedList<ConnectableDot> connectedDots)
    { 
        Disconnect();
    }

    protected virtual void OnDotConnected(ConnectableDot dot)
    {
        UpdateColor();
    }
    public override void Disconnect()
    {
        Deselect();

    }
    //changes the color of the inner dot to the connection's color
    private void UpdateColor()
    {
        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        {
            VisualController.SetInnerColor(color);
        }
    }

    public virtual void Deselect()
    {
        
        VisualController.AnimateDeselectionEffect();
        

    }

    public override void Connect(ConnectableDot dot)
    {
        base.Connect(dot);
        if (dot is IColorable colorable)
            Color = colorable.Color;
    }


}

