
using UnityEngine;

public abstract class BlankDotBase : ConnectableDot
{

    private new BlankDotBaseVisualController VisualController => GetVisualController<BlankDotBaseVisualController>();



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
}

