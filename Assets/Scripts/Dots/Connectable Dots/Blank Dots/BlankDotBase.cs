using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public abstract class BlankDotBase : ConnectableDot, IBlankDot
{


    public override DotType DotType { get; }
    private BlankDotVisualController VisualController
    {
        get
        {
            if (visualController is BlankDotVisualController blankDotVisualController)
            {
                return blankDotVisualController;
            }
            throw new InvalidCastException("Unable to cast base visualController to ClockDotVisualController");

        }
    }

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
    /// <summary>
    /// when dot is disconnected, it starts the deselction animation
    /// </summary>
    /// <param name="dot">The dot that was disconnected</param>

    public void OnDotDisconnected(ConnectableDot dot)
    {

      
        //if any of the blank dots are not set to be hit by the connection then visually disconnect it
        if (!ConnectionManager.ToHit.Contains(this))
        {
            Disconnect();
        }
        //update the inner dot's color to match the connection color
        UpdateColor();
    }
    public void OnConnectionEnded(LinkedList<ConnectableDot> connectedDots)
    { 
        Disconnect();
    }

    protected virtual void OnDotConnected(ConnectableDot dot)
    {
        UpdateColor();
    }
    public override void Disconnect()
    {
        base.Disconnect();
        Deselect();

    }
    //changes the color of the inner dot to the connection's color
    private void UpdateColor()
    {
        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        {
            VisualController.SetColor(color);
        }
    }

    public override void Connect()
    {
        VisualController.AnimateSelectionEffect();
    }

    

    public void Deselect()
    {
        
        VisualController.AnimateDeselectionEffect();
        

    }

    public override void Select()
    {
        VisualController.AnimateSelectionEffect();
    }

}
