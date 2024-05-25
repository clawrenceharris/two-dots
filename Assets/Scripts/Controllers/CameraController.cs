﻿using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Board board;
    private float aspectRatio = 0.625f;
    [SerializeField] private float padding = 1;
    private Camera cam;

    void Start()
    {
        board = FindObjectOfType<Board>();
        cam = GetComponent<Camera>();
        cam.backgroundColor = ColorSchemeManager.CurrentColorScheme.backgroundColor;
    }

    private void Update()
    {
        RepositionCamera((board.Width - 1) * Board.offset , (board.Height - 1) * Board.offset);

    }

    void RepositionCamera(float x, float y)
    {
        Vector3 tempPosition = new(x / 2, y / 2, -1);
        transform.position = tempPosition;
        if (board.Width >= board.Height)
        {
            Camera.main.orthographicSize = (x / 2 + padding) / aspectRatio;
        }
        else
        {
            Camera.main.orthographicSize = y / 2 + padding;
        }

    }


}
