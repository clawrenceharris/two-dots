using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public abstract class BlankDotBase : ConnectableDot, IBlankDot
{


    public override DotType DotType { get; }

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
    
    public virtual void OnDotDisconnected(ConnectableDot dot)
    {
        
        if (dot == this && !ConnectionManager.ConnectedDots.Contains(dot))
            Deselect();

        else if (!ConnectionManager.ConnectedDots.Contains(this))
        {
            Deselect();
        }
        
        //if we disconnect update the inner dot's color to match the connection color
        UpdateColor();
    }
    public virtual void OnConnectionEnded(LinkedList<ConnectableDot> connectedDots)
    { 
        Deselect();
    }

    public virtual void OnDotConnected(ConnectableDot dot)
    {
        UpdateColor();
    }

    //changes the color of the inner dot to the connection's color
    private void UpdateColor()
    {

        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        if (visualController is BlankDotVisualController blankDotVisualController)
        {
            blankDotVisualController.SetColor(color);
        }
    }

    public override void Connect()
    {
        if (visualController is ConnectableDotVisualController cVisualController)
            cVisualController.AnimateSelectionEffect();
    }

    public override void Disconnect()
    {
        //do nothing
    }

    public void Deselect()
    {
        if (visualController is BlankDotVisualController blankDotVisualController)
        {
            blankDotVisualController.AnimateDeselectionEffect();
        }

    }




    public override void Select()
    {
        if (visualController is ConnectableDotVisualController cVisualController)
            cVisualController.AnimateSelectionEffect();
    }

}

