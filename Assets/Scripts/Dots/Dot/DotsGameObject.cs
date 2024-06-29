using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DotsGameObject : MonoBehaviour, IBoardElement
{

    public virtual DotsGameObjectData ReplacementDot => null;
    private int row;
    private int column;
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }

    public T GetVisualController<T>()  where T : class
    {
        if (visualController is T controller)
        {
            return controller;
        }
        throw new InvalidCastException($"Unable to cast base visualController of type {visualController.GetType().Name} to {typeof(T).Name}");
    }

    protected IVisualController visualController;

    public VisualController VisualController => GetVisualController<VisualController>();

    public abstract void InitDisplayController();


    public virtual void Init(int column, int row)
    {
        Column = column;
        Row = row;
        InitDisplayController();
    }


    public void Debug()
    {
        UnityEngine.Debug.Log(name);
        Debug(Color.black);
    }

    public void Debug(Color color)
    {
        visualController.SetColor(color);
    }

}
