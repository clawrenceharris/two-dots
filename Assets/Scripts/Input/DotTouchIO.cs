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
    private static Dot currentDot;
    private float offset = 0.5f;




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
           
            onDotSelected?.Invoke(dot);
        }

        if (Input.GetMouseButton(0) && IsDotAt(worldPos) )
        {
           
            onDotConnected?.Invoke(dot);
        }

        if (Input.GetMouseButtonUp(0))
        {
            onSelectionEnded?.Invoke();
            
           
            
        }
        
    }

    void Update()
    {
        
        if(CommandInvoker.CommandsEnded)
            CheckInput();

    }

   
     
   

}
