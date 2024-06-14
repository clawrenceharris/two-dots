using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DotTouchIO : MonoBehaviour
{
    private ConnectableDot dot;
    

    public static event Action<ConnectableDot> onDotSelected;
    public static event Action<ConnectableDot> onDotConnected;
    public static event Action onSelectionEnded;
    private float offset = 0.5f;
    private static bool selectionEnded;
    public static bool IsInputActive { get; set; }

    private void Awake() => dot = GetComponent<ConnectableDot>();

    private bool IsDotAt(Vector3 worldPosition)
    {
        int column = (int)worldPosition.x;
        int row = (int)worldPosition.y;

        return column == dot.Column && row == dot.Row;
        
    }
   
    private void CheckInput()
    {

        Vector3 worldPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) / Board.offset) + Vector3.one * offset;
        if (Input.GetMouseButtonDown(0) && IsDotAt(worldPos))
        {
            IsInputActive = true;
            selectionEnded = false;
            onDotSelected?.Invoke(dot);
        }

        else if (Input.GetMouseButton(0) && IsDotAt(worldPos))
        {
           
            onDotConnected?.Invoke(dot);
        }

        else if (Input.GetMouseButtonUp(0) && !selectionEnded)
        {
            selectionEnded = true;
            IsInputActive = false;
            onSelectionEnded?.Invoke();

        }

    }

    void Update()
    {
        
        if(CommandInvoker.CommandsEnded)
            CheckInput();

    }
}
