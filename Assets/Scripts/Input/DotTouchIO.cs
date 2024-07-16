using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class DotTouchIO : MonoBehaviour
{
    private ConnectableDot dot;
    

    public static event Action<ConnectableDot> onDotSelected;
    public static event Action<ConnectableDot> onDotConnected;
    public static event Action onSelectionEnded;
    private float Offset
    {
        get
        {
            return 0.5f;
        }
    }
    private static bool selectionEnded;
    public static bool IsInputActive { get; set; }
    private readonly Stack<ConnectableDot> selectedDots = new();
    private void Awake() => dot = GetComponent<ConnectableDot>();

    private bool IsDotAt(Vector3 worldPosition)
    {
        if(dot is not IBoardElement boardElement)
        {
            return false;
        }

        if(dot.HitCount >= dot.HitsToClear)
        {
            return false;
        }

        int column = (int)worldPosition.x;
        int row = (int)worldPosition.y;

        return column == boardElement.Column && row == boardElement.Row;
        
    }
    private IEnumerator AddAndRemoveSelectedDot(ConnectableDot dot)
    {
        selectedDots.Push(dot);
        yield return new WaitForSeconds(DotVisuals.selectionAnimationDuration);
        selectedDots.Pop();
    }
    private void CheckInput()
    {

        Vector3 worldPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) / Board.offset) + Vector3.one * Offset;
        if (Input.GetMouseButtonDown(0) && IsDotAt(worldPos))
        {
            if (selectedDots.Contains(dot))
            {
                return;
            }

            StartCoroutine(AddAndRemoveSelectedDot(dot));
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
        
        if(!CommandInvoker.Instance.IsExecuting)
            CheckInput();

    }
}
